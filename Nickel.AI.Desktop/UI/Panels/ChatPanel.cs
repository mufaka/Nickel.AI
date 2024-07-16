using ImGuiNET;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.Settings;
using Nickel.AI.Desktop.Utilities;
using OllamaSharp;
using OllamaSharp.Models;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class ChatPanel : Panel
    {
        private static string _question = string.Empty;
        private static string _answer = string.Empty;
        private readonly ILogger _logger;

        public ChatPanel(ILogger<ChatPanel> logger)
        {
            _logger = logger;
        }

        public override void HandleUiMessage(UiMessage message)
        {
            if (message != null)
            {
                switch (message.MessageType)
                {
                    case UiMessageConstants.CHAT_SET_QUESTION:
                        SetQuestion(message.Body as string, false);
                        break;
                    case UiMessageConstants.CHAT_ASK_QUESTION:
                        SetQuestion(message.Body as string, true);
                        break;
                }
            }
        }

        private void SetQuestion(string? question, bool triggerLLM)
        {
            if (question != null)
            {
                _question = question.Trim();
                Open = true;

                if (triggerLLM)
                {
                    AskOllama();
                }
            }
        }

        // NOTE: https://raa.is/ImStudio/ is a wysiwyg designer for imgui. Will have to port output to C#.
        public override void DoRender()
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
            if (!string.IsNullOrEmpty(_answer))
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
                try
                {
                    var ollamaEndpointUrl = SettingsManager.ApplicationSettings.Ollama.EndPoint;

                    if (String.IsNullOrWhiteSpace(ollamaEndpointUrl))
                    {
                        _logger.LogInformation("Ollama endpoint is not configured. Using \"http://localhost:11434\" as a default.");
                        ollamaEndpointUrl = "http://localhost:11434";
                    }

                    Uri? ollamaEndpoint;

                    if (!Uri.TryCreate(ollamaEndpointUrl, UriKind.Absolute, out ollamaEndpoint))
                    {
                        _logger.LogInformation($"Ollama configured endpoint is invalid [{ollamaEndpointUrl}]. Using \"http://localhost:11434\" as a default.");
                        ollamaEndpoint = new Uri("http://localhost:11434");
                    }

                    var ollama = new OllamaApiClient(ollamaEndpoint);

                    // ask without giving a context
                    var completionRequest = new GenerateCompletionRequest();
                    completionRequest.Stream = false;
                    completionRequest.Prompt = _question;

                    var model = SettingsManager.ApplicationSettings.Ollama.Model;

                    if (String.IsNullOrWhiteSpace(model))
                    {
                        _logger.LogInformation("Ollama model is not configured. Using \"llama3\" as a default.");
                        model = "llama3";
                    }

                    completionRequest.Model = model;

                    var completionResponse = await ollama.GetCompletion(completionRequest);
                    _answer = completionResponse.Response;
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, ex.Message);
                }
            }
        }
    }
}
