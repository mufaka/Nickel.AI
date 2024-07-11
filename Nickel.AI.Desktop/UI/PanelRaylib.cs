using ImGuiNET;
using Raylib_cs;
using rlImGui_cs;

namespace Nickel.AI.Desktop.UI
{
    public abstract class PanelRaylib : Panel
    {
        public RenderTexture2D ViewTexture;
        public int Width;
        public int Height;

        public PanelRaylib()
        {
            Width = Raylib.GetScreenWidth();
            Height = Raylib.GetScreenHeight();

            ViewTexture = Raylib.LoadRenderTexture(Width, Height);
            Raylib.SetTextureWrap(ViewTexture.Texture, TextureWrap.Repeat);
            Raylib.SetTextureFilter(ViewTexture.Texture, TextureFilter.Point);
        }

        public override void DoRender()
        {
            // NOTE: Reloading a new ViewTexture isn't an option for performance reasons so we need to 
            //       somehow know when the screen was resized.
            var availableSize = ImGui.GetContentRegionAvail();

            if ((int)availableSize.X != Width || (int)availableSize.Y != Height)
            {
                Width = (int)availableSize.X;
                Height = (int)availableSize.Y;
                ViewTexture = Raylib.LoadRenderTexture(Width, Height);
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
