using ImGuiNET;
using Raylib_cs;

namespace Nickel.AI.Desktop.UI;

public class ExamplePanel : Panel
{
    private Texture2D _texture;
    private float _time;

    public override void Attach()
    {
        Console.WriteLine("Attached layer");
        Open = true;
    }

    public override void Detach()
    {
        Console.WriteLine("Detached layer");
        Open = false;
    }

    public override void Render()
    {
        bool isOpen = Open;
        if (!isOpen) return;

        if (ImGui.Begin("Example Window", ref isOpen))
        {
            ImGui.Text("Here's some text.");
            ImGui.Text("Have an image too!");
            ImGui.Image(new IntPtr(_texture.Id), ImGui.GetContentRegionAvail());
            ImGui.End();
        }

        if (!isOpen) Detach();
    }

    public override void Update()
    {
        Raylib.UnloadTexture(_texture);
        _time += Raylib.GetFrameTime();
        int c = (int)(255 * (Math.Sin(_time) + 1) / 2);
        Image image = Raylib.GenImageColor(640, 480, new Color(c, c, c, 255));
        _texture = Raylib.LoadTextureFromImage(image);
        Raylib.UnloadImage(image);
    }
}