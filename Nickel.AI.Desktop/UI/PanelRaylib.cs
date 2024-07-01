using Raylib_cs;
using rlImGui_cs;

namespace Nickel.AI.Desktop.UI
{
    public abstract class PanelRaylib : Panel
    {
        public RenderTexture2D ViewTexture;

        public PanelRaylib()
        {
            // TODO: Get height of menu bar to subtract from texture height
            //ViewTexture = Raylib.LoadRenderTexture((int)DefaultWindowSize.X, (int)DefaultWindowSize.Y - 50);
            ViewTexture = Raylib.LoadRenderTexture(Raylib.GetScreenWidth(), Raylib.GetScreenHeight());
            Raylib.SetTextureWrap(ViewTexture.Texture, TextureWrap.Repeat);
            Raylib.SetTextureFilter(ViewTexture.Texture, TextureFilter.Point);
        }

        public override void DoRender()
        {
            Raylib.BeginTextureMode(ViewTexture);
            Raylib.ClearBackground(Color.Blank);

            RenderRaylib();

            Raylib.EndTextureMode();
            rlImGui.ImageRenderTexture(ViewTexture);
        }

        public abstract void RenderRaylib();
    }
}
