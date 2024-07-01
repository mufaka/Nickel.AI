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

                var wordWrappedAnswer = WordWrap(_answer, windowWidth - 45.0f);

                ImGui.PushID("chat_answer");
                uint bufferLength = Math.Max((uint)wordWrappedAnswer.Length, 4096);

                ImGui.InputTextMultiline("##ans", ref wordWrappedAnswer, bufferLength, new Vector2(windowWidth - 40.0f, windowHeight - 100.0f));

                ImGui.PopID();
            }
        }

        private static string WordWrap(string text, float windowWidth)
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
                buff.Append(WrapLine(line, charsPerLine));
                buff.AppendLine();
            }

            return buff.ToString().Trim();
        }

        private static string WrapLine(string line, int maxChars)
        {
            // NOTE: This predetermines the amount of chunks the line
            //       needs to be split into. Knowing that, we use the
            //       known indices for the splits to check if we are
            //       splitting on a space. If it's not a space and the next
            //       character is a space, split there. If not, work backwards
            //       to find the first occurence of a space and split on that. 

            // How many chunks?
            var chunks = Math.Ceiling((double)line.Length / maxChars);

            // How far back have we gone to find a good split
            int offSet = 0;
            var buff = new StringBuilder();

            // loop through the amount of chunks
            for (int i = 0; i < chunks; i++)
            {
                var chunkStart = i * maxChars - offSet;
                var chunkEnd = Math.Min(chunkStart + maxChars - 1, line.Length - 1);
                var nextChunkBegin = chunkEnd + 1;

                if (line[chunkEnd] != ' ' && nextChunkBegin < line.Length)
                {
                    int space = line.LastIndexOf(' ', chunkEnd, maxChars);

                    if (space != -1)
                    {
                        offSet += chunkEnd - space;
                        chunkEnd = space;
                        // it's possible that we need more chunks because we've split
                        // more times than the original estimate.
                        chunks = chunks + (offSet % maxChars);
                    }
                }

                // because we have possibly added to the chunk count, we may overflow.
                // break out of the loop, we are done.
                if (chunkStart > line.Length - 1 || chunkEnd > line.Length - 1) break;

                // indexer doesn't like maths in syntax, indexer is exclusive so need to add 1 to end
                int exclusiveEnd = chunkEnd + 1;
                buff.AppendLine(line[chunkStart..exclusiveEnd]);
            }

            return buff.ToString();
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
