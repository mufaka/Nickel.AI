using Microsoft.Extensions.Logging;
using OpenCvSharp;
using Raylib_cs;

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
            if (_videoCapture == null)
            {
                // NOTE: This is called every frame so we don't want to spawn a new thread
                //       if we are already trying to initialize the VideoCapture.
                if (!_cameraInitializing)
                {
                    _cameraInitializing = true;
                    // NOTE: There is a delay in initializing the video capture so using a Thread
                    //       here for UI responsiveness.
                    var thread = new Thread(() => { _videoCapture = new VideoCapture(0); });
                    thread.Start();
                }
            }
            else
            {
                var mat = new Mat();
                _videoCapture.Read(mat);
                var image = Raylib.LoadImageFromMemory(".png", mat.ImEncode(".png"));
                _texture = new WebcamTexture();
                _texture.Texture = Raylib.LoadTextureFromImage(image);
                Raylib.UnloadImage(image);
            }

            if (_texture != null)
            {
                Raylib.DrawTexture(_texture.Texture, 0, 0, Color.White);
            }
        }

        public override void DoDetach()
        {
            base.DoDetach();
        }
    }
}
