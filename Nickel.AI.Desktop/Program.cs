using Nickel.AI.Desktop.UI.Panels;
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
            Raylib.SetWindowMinSize(1024, 800);

            //Raylib.MaximizeWindow();
        }

        static void Main(string[] args)
        {
            InitWindow(1600, 1200, "Nickel AI Desktop");

            // NOTE: Panels need to be added before Setup is called.
            SetupPanels();

            UI.UiManager.Setup();

            while (!Raylib.WindowShouldClose() && !UI.UiManager.Quit)
            {
                // Update:
                UI.UiManager.Update();

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

        private static void SetupPanels()
        {
            UI.UiManager.Panels.Add(new ChatPanel());
            UI.UiManager.Panels.Add(new ExamplePanel("Examples", "Data Frame Table"));

            var raylibPanel = new ExampleRaylibPanel();
            raylibPanel.Label = "Raylib drawn inside ImGUI";
            raylibPanel.MenuCategory = "Examples";
            raylibPanel.DefaultWindowSize.X = 800;
            raylibPanel.DefaultWindowSize.Y = 600;

            UI.UiManager.Panels.Add(raylibPanel);

            var textExtractionPanel = new TextExtractionPanel();
            textExtractionPanel.Label = "Text Extraction";
            textExtractionPanel.MenuCategory = "Text";
            textExtractionPanel.DefaultWindowSize.X = 800;
            textExtractionPanel.DefaultWindowSize.Y = 600;

            UI.UiManager.Panels.Add(textExtractionPanel);

            var chunkedDataViewer = new ChunkedDataPanel();
            chunkedDataViewer.Label = "Viewer";
            chunkedDataViewer.MenuCategory = "Data";
            chunkedDataViewer.DefaultWindowSize.X = 800;
            chunkedDataViewer.DefaultWindowSize.Y = 600;

            UI.UiManager.Panels.Add(chunkedDataViewer);
        }
    }
}
