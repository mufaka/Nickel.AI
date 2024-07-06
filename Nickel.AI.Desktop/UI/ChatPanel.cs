using ImGuiNET;
using Nickel.AI.Desktop.Utilities;
using OllamaSharp;
using OllamaSharp.Models;
using System.Numerics;

namespace Nickel.AI.Desktop.UI
{
    public class ChatPanel : Panel
    {
        private static string _question = string.Empty;
        private static string _answer = string.Empty;

        public ChatPanel()
        {
            Label = "Ollama Chat";
            MenuCategory = "Chat";
            DefaultWindowSize.X = 800;
            DefaultWindowSize.Y = 600;
        }

        // NOTE: https://raa.is/ImStudio/ is a wysiwyg designer for imgui. Will have to port output to C#.
        public unsafe override void DoRender()
        {
            // NOTE: ImGui isn't really event driven so things like capturing "Enter" key
            //       inside of InputText isn't straight forward. It returns true if the
            //       text has changed. We don't want to ask Ollama on text changed.
            //       ImGuiInputTextFlags.EnterReturnsTrue changes that behavior to what we want.
            float windowWidth = ImGui.GetWindowWidth();
            float windowHeight = ImGui.GetWindowHeight();
            float characterWidth = ImGui.CalcTextSize("#").X;


            ImGui.SetCursorPos(new Vector2(20, 40));
            ImGui.PushItemWidth(windowWidth - 120);
            ImGui.PushID("chat_question");
            if (ImGui.InputText("", ref _question, 256, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                AskOllama();
            }
            ImGui.PopID();
            ImGui.PopItemWidth();

            ImGui.SetCursorPos(new Vector2(windowWidth - 80, 40));

            // NOTE: This will return true if the button was clicked ...
            if (ImGui.Button("Ask"))
            {
                AskOllama();
            }

            // TODO: Sizing? Border? Word wrap?
            if (!String.IsNullOrEmpty(_answer))
            {
                ImGui.SetCursorPos(new Vector2(20, 80));

                var wordWrappedAnswer = TextUtilities.WordWrap(_answer, characterWidth, windowWidth - 45.0f);

                ImGui.PushID("chat_answer");
                uint bufferLength = Math.Max((uint)wordWrappedAnswer.Length, 4096);

                ImGui.InputTextMultiline("##ans", ref wordWrappedAnswer, bufferLength, new Vector2(windowWidth - 40.0f, windowHeight - 100.0f));

                ImGui.PopID();
            }
        }

        private async void AskOllama()
        {
            if (_question.Length > 0)
            {
                // TODO: make the endpoint and model configurable.
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
