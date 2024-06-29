using Nickel.AI.Desktop.UI;
using Raylib_cs;

namespace Nickel.AI.Desktop
{
    internal class Program
    {
        static void InitWindow(int width, int height, string title)
        {
            Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint |
                                  ConfigFlags.VSyncHint |
                                  ConfigFlags.ResizableWindow |
                                  ConfigFlags.MaximizedWindow);
            Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
            Raylib.InitWindow(width, height, title);
            Raylib.SetWindowMinSize(1024, 800);

            Raylib.MaximizeWindow();
        }

        static void Main(string[] args)
        {
            InitWindow(1600, 1200, "Nickel AI Desktop");

            // NOTE: Panels need to be added before Setup is called.
            /*
            UI.UiManager.Panels.Add(new ExamplePanel("Examples", "Example 1"));
            UI.UiManager.Panels.Add(new ExamplePanel("Examples", "Example 2"));
            UI.UiManager.Panels.Add(new ExamplePanel("Other", "Example 3"));
            UI.UiManager.Panels.Add(new ExamplePanel("Another", "Example 4"));
            */
            UI.UiManager.Panels.Add(new ChatPanel());
            UI.UiManager.Panels.Add(new ExamplePanel("Examples", "Data Frame Table"));

            UI.UiManager.Setup();

            while (!Raylib.WindowShouldClose() && !UI.UiManager.Quit)
            {
                // Update:
                UI.UiManager.Update();

                // Render:
                Raylib.BeginDrawing();
                Raylib.ClearBackground(new Color(32, 32, 32, 100));

                UI.UiManager.Render();

                //Raylib.DrawFPS(0, 0);
                Raylib.EndDrawing();
            }

            UI.UiManager.Shutdown();
            Raylib.CloseWindow();
        }
    }
}
