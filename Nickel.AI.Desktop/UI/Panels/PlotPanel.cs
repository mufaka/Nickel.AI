using Hexa.NET.ImGui;
using Microsoft.Extensions.Logging;
using Raylib_cs;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class PlotPanel : PanelRaylib
    {
        private ILogger _logger;
        private float _rotation = 0.0f;

        public PlotPanel(ILogger<PlotPanel> logger)
        {
            _logger = logger;
        }

        public override void HandleUiMessage(UiMessage message)
        {

        }

        public override void RenderRaylib()
        {
            try
            {
                var screenWidth = ImGui.GetContentRegionAvail().X;
                _rotation += 0.2f;

                if (_rotation > 360.0f)
                {
                    _rotation = 0.0f;
                }

                Raylib.ClearBackground(Color.White);

                Raylib.DrawText("some basic shapes available on raylib", 20, 20, 20, Color.DarkGray);

                // Circle shapes and lines
                Raylib.DrawCircle((int)(screenWidth / 5), 120, 35, Color.DarkBlue);
                Raylib.DrawCircleGradient((int)(screenWidth / 5), 220, 60, Color.Green, Color.SkyBlue);
                Raylib.DrawCircleLines((int)(screenWidth / 5), 340, 80, Color.DarkBlue);

                // Rectangle shapes and lines
                Raylib.DrawRectangle((int)(screenWidth / 4 * 2 - 60), 100, 120, 60, Color.Red);
                Raylib.DrawRectangleGradientH((int)(screenWidth / 4 * 2 - 90), 170, 180, 130, Color.Maroon, Color.Gold);
                Raylib.DrawRectangleLines((int)(screenWidth / 4 * 2 - 40), 320, 80, 60, Color.Orange);  // NOTE: Uses QUADS internally, not lines

                // Triangle shapes and lines
                Raylib.DrawTriangle(new Vector2(screenWidth / 4.0f * 3.0f, 80.0f),
                         new Vector2(screenWidth / 4.0f * 3.0f - 60.0f, 150.0f),
                         new Vector2(screenWidth / 4.0f * 3.0f + 60.0f, 150.0f), Color.Violet);

                Raylib.DrawTriangleLines(new Vector2(screenWidth / 4.0f * 3.0f, 160.0f),
                              new Vector2(screenWidth / 4.0f * 3.0f - 20.0f, 230.0f),
                              new Vector2(screenWidth / 4.0f * 3.0f + 20.0f, 230.0f), Color.DarkBlue);

                // Polygon shapes and lines
                Raylib.DrawPoly(new Vector2(screenWidth / 4.0f * 3, 330), 6, 80, _rotation, Color.Brown);
                Raylib.DrawPolyLines(new Vector2(screenWidth / 4.0f * 3, 330), 6, 90, _rotation, Color.Brown);
                Raylib.DrawPolyLinesEx(new Vector2(screenWidth / 4.0f * 3, 330), 6, 85, _rotation, 6, Color.Beige);

                // NOTE: We draw all LINES based shapes together to optimize internal drawing,
                // this way, all LINES are rendered in a single draw pass
                Raylib.DrawLine(18, 42, (int)(screenWidth - 18), 42, Color.Black);
            }
            catch (Exception ex)
            {
                // TODO: This is a more proper way to log messages as the template
                //       is separate from the parameters. This makes it easier to 
                //       filter / sort logged messages.
                _logger.LogError(ex, "Error rendering RayLib: {message}", ex.Message);
            }
        }
    }
}
