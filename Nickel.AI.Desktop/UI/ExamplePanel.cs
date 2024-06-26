using ImGuiNET;

namespace Nickel.AI.Desktop.UI;

public class ExamplePanel : Panel
{
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
            ImGui.End();
        }

        if (!isOpen) Detach();
    }

    public override void Update()
    {
    }
}