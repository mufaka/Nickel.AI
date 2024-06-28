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
        private static string _answer = string.Empty;
        private static string _wordWrappedAnswer = string.Empty;

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


            ImGui.SetCursorPos(new Vector2(20, 40));
            ImGui.PushItemWidth(windowWidth - 100);
            ImGui.PushID("chat_question");
            if (ImGui.InputText("", ref _question, 256, ImGuiInputTextFlags.EnterReturnsTrue))
            {
                AskOllama();
            }
            ImGui.PopID();
            ImGui.PopItemWidth();

            ImGui.SetCursorPos(new Vector2(windowWidth - 60, 40));

            // NOTE: This will return true if the button was clicked ...
            if (ImGui.Button("Ask"))
            {
                AskOllama();
            }

            // TODO: Sizing? Border? Word wrap?
            if (!String.IsNullOrEmpty(_answer))
            {
                ImGui.SetCursorPos(new Vector2(20, 80));

                _wordWrappedAnswer = WordWrap(_answer, windowWidth - 45.0f);

                try
                {
                    // TODO: Copy / Paste is borked on Windows for this. ctrl-c and ctrl-v hard crash.
                    //       Need to understand the clipboard handling for this in order to trouble shoot.

                    ImGui.PushID("chat_answer");
                    uint bufferLength = Math.Max((uint)_wordWrappedAnswer.Length, 4096);

                    ImGui.InputTextMultiline("##ans", ref _wordWrappedAnswer, bufferLength, new Vector2(windowWidth - 40.0f, windowHeight - 100.0f));

                    ImGui.PopID();
                }
                catch (Exception ex)
                {
                    // NOTE: this exception isn't caught when it's a System.ExecutionEngineException. The error is with
                    //       unsafe/unmanaged code elsewhere in the code. Most likely in ImGui.NET.
                    Console.WriteLine(ex.ToString());
                }
            }
        }

        private string WordWrap(string text, float windowWidth)
        {
            if (String.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            // get the width of a wide character
            float characterWidth = ImGui.CalcTextSize("#").X;

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
