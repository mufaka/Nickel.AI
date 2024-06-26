using ImGuiNET;
using OllamaSharp;
using OllamaSharp.Models;
using System.Numerics;

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
            WindowSize.X = 600;
            WindowSize.Y = 400;
        }

        // NOTE: https://raa.is/ImStudio/ is a wysiwyg designer for imgui. Will have to port output to C#.
        public override void DoRender()
        {
            ImGui.SetCursorPos(new Vector2(20, 40));
            ImGui.PushItemWidth(400);
            ImGui.InputText("", ref _question, 128);
            ImGui.PopItemWidth();

            ImGui.SetCursorPos(new Vector2(430, 40));

            // NOTE: This will return true if the button was clicked ...
            if (ImGui.Button("Ask"))
            {
                AskOllama();
            }

            // TODO: Sizing? Border? Word wrap?
            if (!String.IsNullOrEmpty(_answer))
            {
                ImGui.SetCursorPos(new Vector2(20, 70));
                ImGui.Text(_answer);
            }
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
