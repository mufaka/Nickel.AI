using Microsoft.Data.Analysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.Settings;
using Nickel.AI.Desktop.UI.Panels;
using Raylib_cs;
using System.Reflection;

namespace Nickel.AI.Desktop
{
    internal class App
    {
        private readonly ILogger _logger;

        // NOTE: Note sure if this is the best way to do this but "cheating"
        //       to avoid a major refactor... need the Host to get the services
        //       in order to instantiate through DI.
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        public static IHost Host { get; set; } // This is set in Program.cs as part of the startup. It won't be null, otherwise app will crash on startup.
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        public App(ILogger<App> logger)
        {
            _logger = logger;
        }

        private static void InitWindow(int width, int height, string title)
        {
            Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint |
                                  ConfigFlags.VSyncHint |
                                  ConfigFlags.ResizableWindow);

            Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
            Raylib.InitWindow(width, height, title);
        }

        public void Run()
        {
            OverrideDataFrameGuessKind();

            _logger.LogInformation("Starting Desktop");

            InitWindow(1600, 1200, "Nickel AI Desktop");

            // NOTE: Panels need to be added before Setup is called.
            SetupPanels();

            UI.UiManager.Setup();
            Raylib.SetExitKey(0);

            while (!Raylib.WindowShouldClose() && !UI.UiManager.Quit)
            {
                // Render:
                Raylib.BeginDrawing();
                Raylib.ClearBackground(Color.DarkGray);

                UI.UiManager.Render();

                //Raylib.DrawFPS(0, 0);
                Raylib.EndDrawing();
            }

            UI.UiManager.Shutdown();
            Raylib.CloseWindow();
        }

        // NOTE: When adding Panels be sure to update the default Panel Mask
        //       in Settings.

        private void SetupPanels()
        {
            var panelMask = SettingsManager.ApplicationSettings.PanelMask;

            if ((panelMask & 1) == 1)
            {
                var chatPanel = Host.Services.GetRequiredService<ChatPanel>();
                chatPanel.Label = "Ollama";
                chatPanel.MenuCategory = "Chat";
                chatPanel.DefaultWindowSize.X = 400;
                chatPanel.DefaultWindowSize.Y = 600;
                UI.UiManager.Panels.Add(chatPanel);
            }

            if ((panelMask & 2) == 2)
            {
                var examplePanel = Host.Services.GetRequiredService<ExamplePanel>();
                examplePanel.Label = "Test Panel";
                examplePanel.MenuCategory = "Examples";
                examplePanel.DefaultWindowSize.X = 700;
                examplePanel.DefaultWindowSize.Y = 400;

                UI.UiManager.Panels.Add(examplePanel);
            }

            if ((panelMask & 4) == 4)
            {
                var raylibPanel = Host.Services.GetRequiredService<ExampleRaylibPanel>();
                raylibPanel.Label = "Raylib drawn inside ImGUI";
                raylibPanel.MenuCategory = "Examples";
                raylibPanel.DefaultWindowSize.X = 400;
                raylibPanel.DefaultWindowSize.Y = 400;

                // TODO: Refactor this. It's required for setting up a camera, etc, in 
                //       a manner that won't run every frame. 
                raylibPanel.Setup();

                UI.UiManager.Panels.Add(raylibPanel);
            }

            if ((panelMask & 8) == 8)
            {
                var textExtractionPanel = Host.Services.GetRequiredService<TextExtractionPanel>();
                textExtractionPanel.Label = "Text Extraction";
                textExtractionPanel.MenuCategory = "Text";
                textExtractionPanel.DefaultWindowSize.X = 800;
                textExtractionPanel.DefaultWindowSize.Y = 600;

                UI.UiManager.Panels.Add(textExtractionPanel);
            }

            if ((panelMask & 16) == 16)
            {
                var chunkedDataViewer = Host.Services.GetRequiredService<ChunkedDataPanel>();
                chunkedDataViewer.Label = "Viewer";
                chunkedDataViewer.MenuCategory = "Data";
                chunkedDataViewer.DefaultWindowSize.X = 800;
                chunkedDataViewer.DefaultWindowSize.Y = 600;

                UI.UiManager.Panels.Add(chunkedDataViewer);
            }

            if ((panelMask & 32) == 32)
            {
                // logging panel doesn't get a logger
                var logPanel = new LogPanel();
                logPanel.Label = "Log";
                logPanel.MenuCategory = "Logging";
                logPanel.DefaultWindowSize.X = 800;
                logPanel.DefaultWindowSize.Y = 300;

                UI.UiManager.Panels.Add(logPanel);
            }

            if ((panelMask & 64) == 64)
            {
                var nodePanel = Host.Services.GetRequiredService<ExampleNodePanel>();
                nodePanel.Label = "ImNodes Panel";
                nodePanel.MenuCategory = "Examples";
                nodePanel.DefaultWindowSize.X = 800;
                nodePanel.DefaultWindowSize.Y = 600;
                nodePanel.HasMenuBar = true;

                UI.UiManager.Panels.Add(nodePanel);
            }

            if ((panelMask & 128) == 128)
            {

                var plotPanel = Host.Services.GetRequiredService<ExamplePlotPanel>();
                plotPanel.Label = "ImPlot Panel";
                plotPanel.MenuCategory = "Examples";
                plotPanel.DefaultWindowSize.X = 600;
                plotPanel.DefaultWindowSize.Y = 400;
                plotPanel.HasMenuBar = true;

                UI.UiManager.Panels.Add(plotPanel);
            }

            if ((panelMask & 256) == 256)
            {
                var vectorPanel = Host.Services.GetRequiredService<VectorDbPanel>();
                vectorPanel.Label = "Vector DB Browser";
                vectorPanel.MenuCategory = "Vector DB";
                vectorPanel.DefaultWindowSize.X = 600;
                vectorPanel.DefaultWindowSize.Y = 400;
                vectorPanel.HasMenuBar = true;

                UI.UiManager.Panels.Add(vectorPanel);
            }

            if ((panelMask & 512) == 512)
            {
                var learningPanel = Host.Services.GetRequiredService<LearningPanel>();
                learningPanel.Label = "AIded Learning";
                learningPanel.MenuCategory = "Learn";
                learningPanel.DefaultWindowSize.X = 800;
                learningPanel.DefaultWindowSize.Y = 600;
                learningPanel.HasMenuBar = false;

                UI.UiManager.Panels.Add(learningPanel);
            }

            if ((panelMask & 1024) == 1024)
            {
                var plotPanel = Host.Services.GetRequiredService<PlotPanel>();
                plotPanel.Label = "Plot Test";
                plotPanel.MenuCategory = "Examples";
                plotPanel.DefaultWindowSize = new System.Numerics.Vector2(800, 600);
                plotPanel.HasMenuBar = false;

                UI.UiManager.Panels.Add(plotPanel);
            }

            if ((panelMask & 2048) == 2048)
            {
                var openCVPanel = Host.Services.GetRequiredService<ExampleOpenCVPanel>();
                openCVPanel.Label = "Web Cam Test";
                openCVPanel.MenuCategory = "Examples";
                openCVPanel.DefaultWindowSize = new System.Numerics.Vector2(600, 400);
                openCVPanel.HasMenuBar = false;

                UI.UiManager.Panels.Add(openCVPanel);
            }
        }

        // NOTE: DataFrame type inference is minimal and it doesn't allow for injecting/using your
        // own customer type infererer. The only alternative is to pass your own list of types. This
        // could be resolved with some pre-processing of the files but here we are opting to just return
        // everything as a string type instead as we are just browsing the data (for now). 
        // eg: 19495551212 is a phone number but displays in scientific notation.
        private void OverrideDataFrameGuessKind()
        {

            _logger.LogInformation("Patching Microsoft.DataAnalysis.DataFrame.GuessKind");
            var success = false;

            try
            {
                var typeInfo = typeof(DataFrame);
                var methodInfo = typeInfo.GetMethod("GuessKind", BindingFlags.NonPublic | BindingFlags.Static);


                if (methodInfo != null)
                {
                    var patchMethodInfo = typeof(App).GetMethod("GuessKindOnlyString", BindingFlags.NonPublic | BindingFlags.Static);

                    if (patchMethodInfo != null)
                    {
                        //RuntimeHelpers.PrepareMethod(methodInfo.MethodHandle);
                        //RuntimeHelpers.PrepareMethod(patchMethodInfo.MethodHandle);

                        unsafe
                        {
                            // note, these are only valid if obtained from the same app domain
                            long* targetPtr = (long*)methodInfo.MethodHandle.Value.ToPointer() + 1;
                            long* injectPtr = (long*)patchMethodInfo.MethodHandle.Value.ToPointer() + 1;

                            // swap the target pointer with the new one.
                            *targetPtr = *injectPtr;
                        }

                        success = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
            }

            if (success)
            {
                _logger.LogInformation("Succesfully patched Microsoft.DataAnalysis.DataFrame.GuessKind");
            }
            else
            {
                _logger.LogWarning("Could not patch Microsoft.DataAnalysis.DataFrame.GuessKind");
            }
        }

        // Replacement method for DataFrame type inference.
        private static Type GuessKindOnlyString(int col, List<string[]> read)
        {
            return typeof(string);
        }
    }
}
