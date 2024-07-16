using ImGuiNET;
using Nickel.AI.Desktop.Logging;
using System.Text;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class LogPanel : Panel
    {
        private string _logText = string.Empty;

        private void SetLogText()
        {
            var items = InMemoryLog.Instance.LogItems.Take(100);
            var buff = new StringBuilder();

            foreach (var item in items)
            {
                buff.AppendLine($"{item.LogDate.ToString("yyyy-MM-dd HH:mm:ss")} - {item.Level.ToString()} - {item.Message}");

                if (!String.IsNullOrWhiteSpace(item.Detail))
                {
                    buff.AppendLine(item.Detail);
                }
            }

            _logText = buff.ToString();
        }

        public override void HandleUiMessage(UiMessage message)
        {
            if (message != null)
            {
                switch (message.MessageType)
                {
                    case UiMessageConstants.LOG_SHOW_LOG:
                        Open = true;
                        break;
                    case UiMessageConstants.LOG_CLEAR_LOG:
                        InMemoryLog.Instance.Clear();
                        break;
                }
            }
        }

        public override void DoRender()
        {
            if (ImGui.Button("Clear"))
            {
                InMemoryLog.Instance.Clear();
            }

            ImGui.InputTextMultiline("##log", ref _logText, 1024, ImGui.GetContentRegionAvail());

            if (ImGui.GetFrameCount() % 60 == 0)
            {
                SetLogText();
            }
        }
    }
}
