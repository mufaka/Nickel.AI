using Raylib_cs;

namespace Nickel.AI.Desktop
{
    internal class Program
    {
        static void InitWindow(int width, int height, string title)
        {
            Raylib.SetConfigFlags(ConfigFlags.Msaa4xHint |
                                  ConfigFlags.VSyncHint |
                                  ConfigFlags.ResizableWindow);
            Raylib.SetTraceLogLevel(TraceLogLevel.Warning);
            Raylib.InitWindow(width, height, title);
            Raylib.SetWindowMinSize(800, 600);
        }

        static void Main(string[] args)
        {
            InitWindow(1280, 720, "Nickel AI Desktop");

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
