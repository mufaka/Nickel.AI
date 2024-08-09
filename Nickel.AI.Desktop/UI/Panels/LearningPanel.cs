using Hexa.NET.ImGui;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.Models;
using Nickel.AI.Desktop.Settings;
using OllamaSharp;
using OllamaSharp.Models;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class LearningPanel : Panel
    {
        private ILogger _logger;
        private FlashCards _cards = new FlashCards();
        private string _subject = string.Empty;
        private string _topic = string.Empty;
        private int _selectedCardIndex = -1;


        public LearningPanel(ILogger<LearningPanel> logger)
        {
            _logger = logger;
        }

        public override void HandleUiMessage(UiMessage message)
        {
        }

        public override void DoRender()
        {
            ImGui.InputText("Subject", ref _subject, 256, ImGuiInputTextFlags.None);
            ImGui.InputText("Topic", ref _topic, 256, ImGuiInputTextFlags.None);
            if (ImGui.Button("Create Cards"))
            {
                CreateCards();
            }

            if (_cards.Cards.Count > 0)
            {
                if (ImGui.BeginTabBar("#cards"))
                {
                    if (ImGui.BeginTabItem("Questions"))
                    {
                        RenderQuestions();
                        ImGui.EndTabItem();
                    }

                    if (ImGui.BeginTabItem("Cards"))
                    {
                        RenderCards();
                        ImGui.EndTabItem();
                    }

                    ImGui.EndTabBar();
                }
            }
        }

        private void RenderQuestions()
        {
            if (ImGui.BeginTable("cardtable", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.Resizable))
            {
                ImGui.TableSetupColumn("Question", ImGuiTableColumnFlags.WidthFixed, 400.0f);
                ImGui.TableSetupColumn("Answer", ImGuiTableColumnFlags.WidthStretch);
                ImGui.TableSetupColumn("Detail", ImGuiTableColumnFlags.WidthFixed, 100.0f);
                ImGui.TableHeadersRow();

                int buttonIdx = 0;
                foreach (var card in _cards.Cards)
                {
                    ImGui.TableNextRow();
                    ImGui.TableSetColumnIndex(0);
                    ImGui.TextWrapped(card.Question);

                    ImGui.TableSetColumnIndex(1);
                    ImGui.TextWrapped(card.Answer);

                    ImGui.TableSetColumnIndex(2);
                    ImGui.PushID(buttonIdx);
                    if (ImGui.Button("More"))
                    {
                        MessageQueue.Instance.Enqueue(UiMessageConstants.CHAT_ASK_QUESTION, card.Question);
                    }

                    buttonIdx++;
                }

                ImGui.EndTable();
            }
        }

        private void RenderCards()
        {
            ImGui.TextWrapped("YOU SHOULD REALLY THINK ABOUT SAVING THE DATA");
        }

        private async void CreateCards()
        {
            try
            {
                if (_subject.Length > 0 && _topic.Length > 0)
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
                        completionRequest.Prompt = @$"I am a student learning {_subject}. Generate 30 flash cards for me covering {_topic}. Respond in json with the following format:

                {{
                    ""cards"": [
                        {{ ""question"": """", ""answer"": """" }}  
                    ]
                }}
";

                        var model = SettingsManager.ApplicationSettings.Ollama.Model;

                        if (String.IsNullOrWhiteSpace(model))
                        {
                            _logger.LogInformation("Ollama model is not configured. Using \"llama3.1\" as a default.");
                            model = "llama3.1";
                        }

                        completionRequest.Model = model;

                        var completionResponse = await ollama.GetCompletion(completionRequest);
                        var answer = completionResponse.Response;

                        _cards = FlashCards.FromLlmResponse(answer);
                        _logger.LogInformation("{0} cards created for subject {0}, topic {1}", _cards.Cards.Count, _subject, _topic);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogCritical(ex, ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
