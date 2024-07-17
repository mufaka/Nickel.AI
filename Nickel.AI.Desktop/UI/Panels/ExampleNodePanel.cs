using Hexa.NET.ImGui;
using Hexa.NET.ImNodes;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.UI.Controls;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class ExampleNodePanel : Panel
    {
        private NodeEditor editor = new();
        private ILogger _logger;

        public ExampleNodePanel(ILogger<ExampleNodePanel> logger)
        {
            _logger = logger;
            editor.Initialize();
            var node1 = editor.CreateNode("Node");
            node1.CreatePin(editor, "In", PinKind.Input, PinType.DontCare, ImNodesPinShape.Circle);
            var out1 = node1.CreatePin(editor, "Out", PinKind.Output, PinType.DontCare, ImNodesPinShape.Circle);
            var node2 = editor.CreateNode("Node");
            var in2 = node2.CreatePin(editor, "In", PinKind.Input, PinType.DontCare, ImNodesPinShape.Circle);
            var out2 = node2.CreatePin(editor, "Out", PinKind.Output, PinType.DontCare, ImNodesPinShape.Circle);
            var node3 = editor.CreateNode("Node");
            var in3 = node3.CreatePin(editor, "In", PinKind.Input, PinType.DontCare, ImNodesPinShape.Circle);
            node3.CreatePin(editor, "Out", PinKind.Output, PinType.DontCare, ImNodesPinShape.Circle);
            editor.CreateLink(out1, in2);
            editor.CreateLink(out1, in3);
            editor.CreateLink(out2, in3);
        }

        public override void HandleUiMessage(UiMessage message)
        {
        }

        public override void DoRender()
        {

            if (ImGui.BeginMenuBar())
            {
                if (ImGui.MenuItem("New Node"))
                {
                    var node = editor.CreateNode("Node");
                    node.CreatePin(editor, "In", PinKind.Input, PinType.DontCare, ImNodesPinShape.Circle);
                    node.CreatePin(editor, "Out", PinKind.Output, PinType.DontCare, ImNodesPinShape.Circle);
                    _logger.LogInformation("New Node button clicked");
                }
                ImGui.EndMenuBar();
            }

            // How do we size the editor to the available content size?
            editor.Draw();
        }
    }
}
