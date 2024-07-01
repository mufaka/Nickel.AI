using ImGuiNET;
using Raylib_cs;

namespace Nickel.AI.Desktop.UI
{
    public class ExampleRaylibPanel : PanelRaylib
    {
        private int _width;
        private int _height;
        private Random _random = new Random(Guid.NewGuid().GetHashCode());
        private List<Circle> _circles = new List<Circle>();

        public override void RenderRaylib()
        {
            _width = (int)ImGui.GetWindowWidth();
            _height = (int)ImGui.GetWindowHeight();

            if (_circles.Count == 0)
            {
                _circles = GenerateCircles();
            }

            DrawGrid();

            if (ImGui.Button("Reset"))
            {
                _circles = GenerateCircles();
            }
        }

        private void DrawGrid()
        {
            const int cellSize = 20;

            for (int i = cellSize; i < _height; i += cellSize)
            {
                Raylib.DrawLine(0, i, _width, i, Color.DarkGray);
            }

            for (int i = cellSize; i < _width; i += cellSize)
            {
                Raylib.DrawLine(i, 0, i, _height, Color.DarkGray);
            }

            //if (ImGui.GetFrameCount() % 5 == 0)
            {
                foreach (var circle in _circles)
                {
                    MoveCircle(circle);

                    if (circle.X < 0) circle.X = 0;
                    if (circle.X > _width) circle.X = _width;
                    if (circle.Y < 0) circle.Y = 0;
                    if (circle.Y > _height) circle.Y = _height;
                }
            }

            foreach (var circle in _circles)
            {
                Raylib.DrawCircle(circle.X, circle.Y, circle.Size, new Color(circle.R, circle.G, circle.B, 255));
            }
        }

        private void MoveCircle(Circle circle)
        {
            var movement = _random.Next(0, 9);

            /*
                0 1 2
                3 4 5
                6 7 8
            */
            switch (movement)
            {
                case 0:
                    circle.X -= 1;
                    circle.Y -= 1;
                    break;
                case 1:
                    circle.Y -= 1;
                    break;
                case 2:
                    circle.X += 1;
                    circle.Y -= 1;
                    break;
                case 3:
                    circle.X -= 1;
                    break;
                case 5:
                    circle.X += 1;
                    break;
                case 6:
                    circle.X -= 1;
                    circle.Y += 1;
                    break;
                case 7:
                    circle.Y += 1;
                    break;
                case 8:
                    circle.X += 1;
                    circle.Y += 1;
                    break;
            }
        }

        private class Circle
        {
            public int X, Y;
            public int R, G, B;
            public int Size;
        }

        private List<Circle> GenerateCircles()
        {
            var circles = new List<Circle>();

            for (int i = 0; i < 1000; i++)
            {
                var circle = new Circle();

                circle.X = _random.Next(0, _width);
                circle.Y = _random.Next(0, _height);
                circle.Size = _random.Next(2, 10);

                circle.R = _random.Next(0, 255);
                circle.G = _random.Next(0, 255);
                circle.B = _random.Next(0, 255);

                circles.Add(circle);
            }

            return circles;
        }

        public override void Update()
        {
        }
    }
}
