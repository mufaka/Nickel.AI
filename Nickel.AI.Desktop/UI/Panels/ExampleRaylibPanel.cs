using ImGuiNET;
using Microsoft.Extensions.Logging;
using Raylib_cs;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class ExampleRaylibPanel : PanelRaylib
    {
        private Camera3D _camera3D = new Camera3D();
        private Vector3 _cubePosition = new Vector3(0.0f, 0.0f, 0.0f);
        private Font _font;
        private ILogger _logger;

        public ExampleRaylibPanel(ILogger<ExampleRaylibPanel> logger)
        {
            _logger = logger;
        }

        private Vector2[] _helpTextVectors =
        {
            new Vector2(20, 20),
            new Vector2(40, 40),
            new Vector2(40, 60),
            new Vector2(40, 80)
        };

        private float _fontSize = 16.0f;
        private float _fontSpacing = 1.0f;

        public override void Setup()
        {
            _camera3D.Position = new Vector3(10.0f, 10.0f, 10.0f);
            _camera3D.Target = new Vector3(0.0f, 0.0f, 0.0f);
            _camera3D.Up = new Vector3(0.0f, 1.0f, 0.0f);
            _camera3D.FovY = 45;
            _camera3D.Projection = CameraProjection.Perspective;

            _font = Raylib.LoadFont(Path.Combine("Resources", "JetBrainsMono-Medium.ttf"));
        }

        public override void RenderRaylib()
        {
            if (ImGui.IsWindowHovered())
            {
                // Only update camera when right click held or moving scroll wheel
                if (Raylib.IsMouseButtonDown(MouseButton.Right))
                {
                    Raylib.UpdateCamera(ref _camera3D, CameraMode.Free);
                }

                if (Raylib.GetMouseWheelMove() != 0.0f)
                {
                    Raylib.UpdateCamera(ref _camera3D, CameraMode.Free);
                }

                // reset target to look at cube.
                if (Raylib.IsKeyPressed(KeyboardKey.Z)) _camera3D.Target = _cubePosition;
            }

            Raylib.ClearBackground(Color.White);

            Raylib.BeginMode3D(_camera3D);

            Raylib.DrawCube(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Red);
            Raylib.DrawCubeWires(_cubePosition, 2.0f, 2.0f, 2.0f, Color.Black);

            Raylib.DrawGrid(10, 1.0f);
            Raylib.EndMode3D();

            Raylib.DrawRectangle(10, 10, 320, 93, Raylib.Fade(Color.SkyBlue, 0.5f));
            Raylib.DrawRectangleLines(10, 10, 320, 93, Color.Blue);

            Raylib.DrawTextEx(_font, "Free camera default controls:", _helpTextVectors[0], _fontSize, _fontSpacing, Color.Black);
            Raylib.DrawTextEx(_font, "- Mouse Wheel to Zoom in-out", _helpTextVectors[1], _fontSize, _fontSpacing, Color.DarkGray);
            Raylib.DrawTextEx(_font, "- Right click to Pan", _helpTextVectors[2], _fontSize, _fontSpacing, Color.DarkGray);
            Raylib.DrawTextEx(_font, "- Z to move to (0, 0, 0)", _helpTextVectors[3], _fontSize, _fontSpacing, Color.DarkGray);
        }
    }
}
