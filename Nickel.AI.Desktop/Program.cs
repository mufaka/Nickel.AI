using Microsoft.Data.Analysis;
using Nickel.AI.Desktop.UI.Panels;
using Raylib_cs;
using System.Reflection;

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
            OverrideDataFrameGuessKind();
            //return;
            InitWindow(1600, 1200, "Nickel AI Desktop");

            // NOTE: Panels need to be added before Setup is called.
            SetupPanels();

            UI.UiManager.Setup();

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

        // NOTE: DataFrame type inference is minimal and it doesn't allow for injecting/using your
        // own customer type infererer. The only alternative is to pass your own list of types. This
        // could be resolved with some pre-processing of the files but here we are opting to just return
        // everything as a string type instead as we are just browsing the data (for now). 
        // eg: 19495551212 is a phone number but displays in scientific notation.
        private static void OverrideDataFrameGuessKind()
        {
            Console.Write("Patching Microsoft.DataAnalysis.DataFrame.GuessKind ... ");
            var success = false;

            try
            {
                var typeInfo = typeof(DataFrame);
                var methodInfo = typeInfo.GetMethod("GuessKind", BindingFlags.NonPublic | BindingFlags.Static);


                if (methodInfo != null)
                {
                    var patchMethodInfo = typeof(Program).GetMethod("GuessKindOnlyString", BindingFlags.NonPublic | BindingFlags.Static);

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
                // TODO: Warn
            }

            Console.WriteLine(success ? "Success" : "Failed");
        }

        // Replacement method for DataFrame type inference.
        private static Type GuessKindOnlyString(int col, List<string[]> read)
        {
            return typeof(string);
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
            raylibPanel.Setup();

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
