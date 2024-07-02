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
                    circle.Move(_random);

                    if (circle.X < 0) circle.X = 0;
                    if (circle.X > _width) circle.X = _width;
                    if (circle.Y < 0) circle.Y = 0;
                    if (circle.Y > _height) circle.Y = _height;
                }
            }

            foreach (var circle in _circles)
            {
                Raylib.DrawCircle(circle.X, circle.Y, circle.Size, circle.Color);
            }
        }

        private class Circle
        {
            public int X, Y;
            public Color Color;
            public int Size;
            public bool Alive = true;

            private int _movementCount = 0;
            private int _lastMovement = 4; // no where

            public void Move(Random random)
            {
                if (!Alive) return;

                // randomize the amount of moves in the same direction
                var directionCount = random.Next(1, 100);

                if (_movementCount < directionCount)
                {
                    _movementCount++;
                }
                else
                {
                    _movementCount = 0;
                    _lastMovement = random.Next(0, 9);
                }

                var growthCheck = random.Next(1, 1000);

                if (growthCheck == 800)
                {
                    Size = Size - 1;
                    Color = new Color(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255), 255);

                    if (Size < 2)
                    {
                        Alive = false;
                    }
                }

                switch (_lastMovement)
                {
                    case 0:
                        X -= 1;
                        Y -= 1;
                        break;
                    case 1:
                        Y -= 1;
                        break;
                    case 2:
                        X += 1;
                        Y -= 1;
                        break;
                    case 3:
                        X -= 1;
                        break;
                    case 5:
                        X += 1;
                        break;
                    case 6:
                        X -= 1;
                        Y += 1;
                        break;
                    case 7:
                        Y += 1;
                        break;
                    case 8:
                        X += 1;
                        Y += 1;
                        break;
                }

            }
        }

        private List<Circle> GenerateCircles()
        {
            var circles = new List<Circle>();
            var circleCount = _random.Next(200, 1001);


            for (int i = 0; i < circleCount; i++)
            {
                var circle = new Circle();

                circle.X = _random.Next(0, _width);
                circle.Y = _random.Next(0, _height);
                circle.Size = _random.Next(4, 12);
                circle.Color = new Color(_random.Next(0, 255), _random.Next(0, 255), _random.Next(0, 255), 255);

                circles.Add(circle);
            }

            return circles;
        }

        public override void Update()
        {
        }
    }
}
