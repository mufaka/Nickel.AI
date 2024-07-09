using ImGuiNET;
using Raylib_cs;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class ExampleRaylibPanel : PanelRaylib
    {
        private Random _random = new Random(Guid.NewGuid().GetHashCode());
        private List<Circle> _circles = new List<Circle>();

        public override void RenderRaylib()
        {
            //Width = (int)ImGui.GetWindowWidth();
            //Height = (int)ImGui.GetWindowHeight();

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

            //for (int i = cellSize; i < _height; i += cellSize)
            for (int i = 1; i < Height; i += cellSize)
            {
                Raylib.DrawLine(0, i, Width, i, Color.DarkGray);
            }

            //for (int i = cellSize; i < _width; i += cellSize)
            for (int i = 1; i < Width; i += cellSize)
            {
                Raylib.DrawLine(i, 0, i, Height, Color.DarkGray);
            }

            //if (ImGui.GetFrameCount() % 5 == 0)
            {
                foreach (var circle in _circles)
                {
                    circle.Move(_random);

                    if (circle.X < 0) circle.X = 0;
                    if (circle.X > Width) circle.X = Width;
                    if (circle.Y < 0) circle.Y = 0;
                    if (circle.Y > Height) circle.Y = Height;
                }
            }

            foreach (var circle in _circles)
            {
                Raylib.DrawCircleGradient(circle.X, circle.Y, circle.Size, circle.Color, circle.InnerColor);
            }
        }

        private class Circle
        {
            public int X, Y;
            public Color Color;
            public Color InnerColor;
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

            float minCount = 100.0f;
            float maxCount = 5000.0f;
            float biggestSize = 20.0f;
            float smallestSize = 4.0f;

            var circleCount = _random.Next((int)minCount, (int)maxCount + 1);

            // scale max size based on amount of circles. less circles means bigger size
            float factor = (circleCount - minCount) / (maxCount - minCount);
            float maxSize = biggestSize - (biggestSize - smallestSize) * factor;

            for (int i = 0; i < circleCount; i++)
            {
                var circle = new Circle();

                circle.X = _random.Next(0, Width);
                circle.Y = _random.Next(0, Height);
                circle.Size = _random.Next((int)smallestSize, (int)maxSize + 1);

                circle.Color = new Color(_random.Next(0, 256), _random.Next(0, 256), _random.Next(0, 256), 255);
                circle.InnerColor = new Color(_random.Next(0, 256), _random.Next(0, 256), _random.Next(0, 256), 255);

                circles.Add(circle);
            }

            return circles;
        }

        public override void Update()
        {
        }
    }
}
