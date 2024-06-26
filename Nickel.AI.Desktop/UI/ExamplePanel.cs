using ImGuiNET;

namespace Nickel.AI.Desktop.UI;

public class ExamplePanel : Panel
{
    public ExamplePanel(string menu = "Examples", string label = "Label")
    {
        DefaultWindowSize.X = 200;
        DefaultWindowSize.Y = 200;
        MenuCategory = menu;
        Label = label;
    }

    public override void DoRender()
    {
        ImGui.Text("Here's some text.");
        ImGui.End();
    }

    public override void Update()
    {
    }
}