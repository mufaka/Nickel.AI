using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace Nickel.AI.Desktop.UI
{
    public abstract class PanelRaylib : Panel
    {
        public RenderTexture2D ViewTexture;
        private int _width;
        private int _height;

        public PanelRaylib()
        {
            _width = Raylib.GetScreenWidth();
            _height = Raylib.GetScreenHeight();

            ViewTexture = Raylib.LoadRenderTexture(_width, _height);
            Raylib.SetTextureWrap(ViewTexture.Texture, TextureWrap.Repeat);
            Raylib.SetTextureFilter(ViewTexture.Texture, TextureFilter.Point);
        }

        public override void DoRender()
        {
            // NOTE: Reloading a new ViewTexture isn't an option for performance reasons so we need to 
            //       somehow know when the screen was resized.
            var availableSize = ImGui.GetContentRegionAvail();

            if ((int)availableSize.X != _width || (int)availableSize.Y != _height)
            {
                _width = (int)availableSize.X;
                _height = (int)availableSize.Y;
                ViewTexture = Raylib.LoadRenderTexture(_width, _height);
            }

            Raylib.BeginTextureMode(ViewTexture);
            Raylib.ClearBackground(Color.Blank);

            RenderRaylib();

            Raylib.EndTextureMode();
            rlImGui.ImageRenderTexture(ViewTexture);
        }

        public abstract void RenderRaylib();
    }
}
