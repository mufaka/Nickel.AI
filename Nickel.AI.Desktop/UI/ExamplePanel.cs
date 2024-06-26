using ImGuiNET;

namespace Nickel.AI.Desktop.UI;

public class ExamplePanel : Panel
{
    public ExamplePanel(string menu = "Examples", string label = "Label")
    {
        Menu = menu;
        Label = label;
    }

    public override void Attach()
    {
        Open = true;
    }

    public override void Detach()
    {
        Open = false;
    }

    public override void Render()
    {
        bool isOpen = Open;
        if (!isOpen) return;

        if (ImGui.Begin(Label, ref isOpen))
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