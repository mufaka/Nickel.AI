using Hexa.NET.ImPlot;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class ExamplePlotPanel : Panel
    {
        private ILogger _logger;
        private RingBuffer Frame = new(512);

        public ExamplePlotPanel(ILogger<ExamplePlotPanel> logger)
        {
            _logger = logger;
            Time.Initialize();
        }

        public override void HandleUiMessage(UiMessage message)
        {
        }

        public override void DoRender()
        {
            const int shade_mode = 2;
            const float fill_ref = 0;
            double fill = shade_mode == 0 ? -double.PositiveInfinity : shade_mode == 1 ? double.PositiveInfinity : fill_ref;

            Frame.Add(Time.Delta * 1000);

            // ImPlot.SetNextAxesToFit(); This doesn't seem to work?
            ImPlot.SetNextAxesLimits(0, 512, 0, 50);
            if (ImPlot.BeginPlot("Frame", new Vector2(-1, 0), ImPlotFlags.NoInputs))
            {
                ImPlot.PushStyleVar(ImPlotStyleVar.FillAlpha, 0.25f);
                ImPlot.PlotShaded("Total", ref Frame.Values[0], Frame.Length, fill, 1, 0, ImPlotShadedFlags.None, Frame.Head);
                ImPlot.PopStyleVar();

                ImPlot.PlotLine("Total", ref Frame.Values[0], Frame.Length, 1, 0, ImPlotLineFlags.None, Frame.Head);
                ImPlot.EndPlot();
            }

            Time.FrameUpdate();
        }

        // copied from https://github.com/HexaEngine/Hexa.NET.ImGui/blob/master/ExampleD3D11/ImPlotDemo/RingBuffer.cs
        public unsafe struct RingBuffer
        {
            private readonly float[] values;
            private readonly int length;
            private int tail = 0;
            private int head;

            public RingBuffer(int length)
            {
                values = new float[length];
                this.length = length;
            }

            public float[] Values => values;

            public int Length => length;

            public int Tail => tail;

            public int Head => head;

            public void Add(float value)
            {
                //Console.WriteLine($"Adding {value} to RingBuffer.");
                if (value < 0)
                    value = 0;
                values[tail] = value;

                tail++;

                if (tail == length)
                {
                    tail = 0;
                }
                if (tail < 0)
                {
                    tail = length - 1;
                }

                head = (tail - length) % length;

                if (head < 0)
                    head += length;
            }
        }

        public static class Time
        {
            private static long last;
            private static float fixedTime;
            private static float cumulativeFrameTime;

            // Properties
            public static float Delta { get; private set; }

            public static float CumulativeFrameTime { get => cumulativeFrameTime; }

            public static int FixedUpdateRate { get; set; } = 3;

            public static float FixedUpdatePerSecond => FixedUpdateRate / 1000F;

            public static event EventHandler? FixedUpdate;

            // Public Methods
            public static void Initialize()
            {
                last = Stopwatch.GetTimestamp();
                fixedTime = 0;
                cumulativeFrameTime = 0;
            }

            public static void FrameUpdate()
            {
                long now = Stopwatch.GetTimestamp();
                double deltaTime = ((double)now - last) / Stopwatch.Frequency;

                // Calculate the frame time by the time difference over the timer speed resolution.
                Delta = (float)deltaTime;
                cumulativeFrameTime += Delta;
                fixedTime += Delta;
                if (deltaTime == 0 || deltaTime < 0)
                {
                    throw new InvalidOperationException();
                }

                while (fixedTime > FixedUpdatePerSecond)
                {
                    fixedTime -= FixedUpdatePerSecond;
                    FixedUpdate?.Invoke(null, EventArgs.Empty);
                }

                last = now;
            }
        }
    }
}
