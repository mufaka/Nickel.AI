using Hexa.NET.ImGui;
using Microsoft.Extensions.Logging;
using OpenCvSharp;
using Raylib_cs;
using TesseractOCR;
using TesseractOCR.Enums;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class ExampleOpenCVPanel : PanelRaylib
    {
        private ILogger _logger;
        private WebcamTexture? _texture;
        private VideoCapture? _videoCapture;
        private bool _cameraInitializing = false;

        private class WebcamTexture
        {
            public Texture2D Texture { get; set; }
        }

        public ExampleOpenCVPanel(ILogger<ExampleOpenCVPanel> logger)
        {
            _logger = logger;
        }

        public override void HandleUiMessage(UiMessage message)
        {
        }

        public override void RenderRaylib()
        {
            if (_texture != null)
            {
                Raylib.DrawTexture(_texture.Texture, 0, 0, Color.White);
            }

            if (_videoCapture == null)
            {
                // NOTE: This is called every frame so we don't want to spawn a new thread
                //       if we are already trying to initialize the VideoCapture.
                if (!_cameraInitializing)
                {
                    _cameraInitializing = true;
                    // NOTE: There is a delay in initializing the video capture so using a Thread
                    //       here for UI responsiveness.
                    var thread = new Thread(() =>
                    {
                        _videoCapture = new VideoCapture(0);
                        _videoCapture.Saturation = 50;
                    });
                    thread.Start();
                }
            }
            else
            {
                var mat = new Mat();
                var grayMat = new Mat();
                _videoCapture.Read(mat);

                Cv2.CvtColor(mat, grayMat, ColorConversionCodes.BGR2GRAY);
                var imageBytes = grayMat.ImEncode(".png");
                var image = Raylib.LoadImageFromMemory(".png", imageBytes);

                _texture = new WebcamTexture();
                _texture.Texture = Raylib.LoadTextureFromImage(image);

                Raylib.UnloadImage(image);

                if (ImGui.Button("Read"))
                {
                    ReadTextFromImage(imageBytes);
                }
            }
        }

        private void ReadTextFromImage(byte[] imageBytes)
        {
            // TODO: This can probably be scoped at class level
            using var engine = new Engine(@"./TesseractData", Language.English, EngineMode.Default);
            using var img = TesseractOCR.Pix.Image.LoadFromMemory(imageBytes);
            using var page = engine.Process(img);

            Console.WriteLine(page.Text);

            /*
            foreach (var block in page.Layout)
            {
                foreach (var paragraph in block.Paragraphs)
                {
                    foreach (var textLine in paragraph.TextLines)
                    {
                        if (textLine.Confidence > 50.0 && !String.IsNullOrWhiteSpace(textLine.Text))
                        {
                            Console.Write($"{textLine.Confidence}\t\t{textLine.Text}");
                        }
                    }
                }
            }
            */

        }

        public override void DoDetach()
        {
            base.DoDetach();
            if (_videoCapture != null)
            {
                if (!_videoCapture.IsDisposed)
                {
                    _videoCapture.Dispose();
                }
            }
            _videoCapture = null;
        }
    }
}
