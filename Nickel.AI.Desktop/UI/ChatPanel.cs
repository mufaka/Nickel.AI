using ImGuiNET;
using OllamaSharp;
using OllamaSharp.Models;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace Nickel.AI.Desktop.UI
{
    public class ChatPanel : Panel
    {
        private static string _question = string.Empty;
        private string _answer = string.Empty;

        public ChatPanel()
        {
            Label = "Ollama Chat";
            MenuCategory = "Chat";
            DefaultWindowSize.X = 600;
            DefaultWindowSize.Y = 400;
        }

        // NOTE: https://raa.is/ImStudio/ is a wysiwyg designer for imgui. Will have to port output to C#.
        public override void DoRender()
        {
            // NOTE: ImGui isn't really event driven so things like capturing "Enter" key
            //       inside of InputText isn't straight forward. It returns true if the
            //       text has changed. We don't want to ask Ollama on text changed.
            //       ImGuiInputTextFlags.EnterReturnsTrue changes that behavior to what we want.

            ImGui.SetCursorPos(new Vector2(20, 40));
            ImGui.PushItemWidth(500);
            if (ImGui.InputText("", ref _question, 128, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                AskOllama();
            }
            ImGui.PopItemWidth();

            ImGui.SetCursorPos(new Vector2(530, 40));

            // NOTE: This will return true if the button was clicked ...
            if (ImGui.Button("Ask"))
            {
                AskOllama();
            }

            // TODO: Sizing? Border? Word wrap?
            if (!String.IsNullOrEmpty(_answer))
            {
                ImGui.SetCursorPos(new Vector2(20, 70));
                ImGui.Text(WordWrap(_answer));
            }
        }

        private string WordWrap(string text)
        {
            if (String.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // get the width of a wide character
            float characterWidth = ImGui.CalcTextSize("#").X;
            float windowWidth = ImGui.GetWindowSize().X;

            // leave a couple character margin for error in window width
            int charsPerLine = (int)Math.Floor(windowWidth / characterWidth) - 2;

            var lines = Regex.Split(text, "\r\n|\r|\n");
            var buff = new StringBuilder();

            foreach (string line in lines)
            {
                if (!String.IsNullOrWhiteSpace(line))
                {
                    var chunks = line.Chunk(charsPerLine).Select(x => new string(x)).ToList();

                    buff.AppendLine(String.Join("\r\n  ", chunks));
                    buff.AppendLine();
                }
            }

            return buff.ToString().Trim();
        }

        private async void AskOllama()
        {
            if (_question.Length > 0)
            {
                var ollamaEndpoint = new Uri("http://localhost:11434");
                var ollama = new OllamaApiClient(ollamaEndpoint);

                // ask without giving a context
                var completionRequest = new GenerateCompletionRequest();
                completionRequest.Stream = false;
                completionRequest.Prompt = _question;
                completionRequest.Model = "llama3";

                var completionResponse = await ollama.GetCompletion(completionRequest);
                _answer = completionResponse.Response;
            }
        }

        public override void Update()
        {

        }
    }
}
