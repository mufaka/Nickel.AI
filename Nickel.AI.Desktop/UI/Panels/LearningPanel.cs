using Hexa.NET.ImGui;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.External.Mochi;
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
        private int _selectedCardIndex = 0;
        private int _cardSide = 0;

        private string _mochiKey = string.Empty;
        private MochiClient? _mochiClient;
        private List<MochiDeck>? _decks;
        private MochiDeck? _selectedDeck;
        private bool _fetchingDecks = false;
        private string _deckName = string.Empty;

        public LearningPanel(ILogger<LearningPanel> logger)
        {
            _logger = logger;
            _mochiKey = SettingsManager.ApplicationSettings.Mochi.ApiKey;

            if (String.IsNullOrEmpty(_mochiKey))
            {
                _mochiKey = string.Empty;
            }
            else
            {
                _mochiClient = new MochiClient(_mochiKey);
            }
        }

        public override void HandleUiMessage(UiMessage message)
        {
        }

        public override void DoRender()
        {
            try
            {
                ImGui.InputText("Subject", ref _subject, 256, ImGuiInputTextFlags.None);
                ImGui.InputText("Topic", ref _topic, 256, ImGuiInputTextFlags.None);
                if (ImGui.Button("Create Cards"))
                {
                    if (!String.IsNullOrWhiteSpace(_subject) && !String.IsNullOrWhiteSpace(_topic))
                    {
                        _selectedCardIndex = 0;
                        _cardSide = 0;
                        _fetchingDecks = false;
                        CreateCards();
                    }
                }

                if (_cards.Cards.Count > 0)
                {
                    if (ImGui.BeginTabBar("CardTabBar", ImGuiTabBarFlags.None))
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

                        if (ImGui.BeginTabItem("Mochi"))
                        {
                            RenderMochiIntegration();
                            ImGui.EndTabItem();
                        }

                        ImGui.EndTabBar();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                    ImGui.PopID();

                    buttonIdx++;
                }

                ImGui.EndTable();
            }
        }

        private void RenderCards()
        {
            var availableSize = ImGui.GetContentRegionAvail();
            var card = _cards.Cards[_selectedCardIndex];
            var cardLabel = _cardSide == 0 ? card.Question : card.Answer;

            if (ImGui.Button(cardLabel, new System.Numerics.Vector2(availableSize.X, 300.0f)))
            {
                // flip card
                _cardSide = _cardSide == 0 ? 1 : 0;
            }

            if (ImGui.Button("Previous"))
            {
                _selectedCardIndex--;
                _cardSide = 0;

                if (_selectedCardIndex < 0)
                {
                    _selectedCardIndex = _cards.Cards.Count - 1;
                }
            }

            ImGui.SameLine();

            if (ImGui.Button("Next"))
            {
                _selectedCardIndex++;
                _cardSide = 0;

                if (_selectedCardIndex > _cards.Cards.Count - 1)
                {
                    _selectedCardIndex = 0;
                }
            }

            ImGui.SameLine();

            if (ImGui.Button("Detail"))
            {
                MessageQueue.Instance.Enqueue(UiMessageConstants.CHAT_ASK_QUESTION, card.Question);
            }
        }

        private void RenderMochiIntegration()
        {
            ImGui.InputText("Mochi API Key", ref _mochiKey, 100, ImGuiInputTextFlags.Password);
            ImGui.SameLine();
            if (ImGui.Button("Save Key"))
            {
                SettingsManager.ApplicationSettings.Mochi.ApiKey = _mochiKey;
                SettingsManager.SaveAll();

                if (!String.IsNullOrEmpty(_mochiKey))
                {
                    _mochiClient = new MochiClient(_mochiKey);
                    _decks = null;
                    _selectedDeck = null;
                    _fetchingDecks = false;
                }
            }

            if (_mochiClient != null)
            {
                if (_decks == null)
                {
                    // NOTE: GetMochiDecks is an async call so this 
                    //       will get called every frame until decks
                    //       are populated without the _fetchingDecks check
                    if (!_fetchingDecks)
                    {
                        _fetchingDecks = true;
                        GetMochiDecks();
                    }
                }
                else
                {
                    // TODO: allow for creation when no decks are present in Mochi
                    if (_decks.Count > 0 && _selectedDeck != null)
                    {
                        if (ImGui.BeginCombo("Parent Deck", _selectedDeck.Name, ImGuiComboFlags.WidthFitPreview))
                        {
                            foreach (MochiDeck deck in _decks)
                            {
                                var isSelected = deck.Id == _selectedDeck.Id;

                                if (ImGui.Selectable(deck.Name, isSelected))
                                {
                                    _selectedDeck = deck;
                                }

                                if (isSelected)
                                {
                                    ImGui.SetItemDefaultFocus();
                                }
                            }

                            ImGui.EndCombo();
                        }

                        ImGui.InputText("Deck Name", ref _deckName, 100, ImGuiInputTextFlags.None);

                        if (!String.IsNullOrWhiteSpace(_deckName))
                        {
                            if (ImGui.Button("Upload Cards"))
                            {
                                UploadCards(_deckName);
                                _deckName = string.Empty;
                            }
                        }
                    }
                }
            }
        }

        private bool DeckAlreadyExists(string name)
        {
            var existing = _decks.Where(d => d.Name == name).FirstOrDefault();
            return existing != null;
        }

        private async void UploadCards(string deckName)
        {
            try
            {
                var mochiClient = new MochiClient(SettingsManager.ApplicationSettings.Mochi.ApiKey);

                // TODO: allow for uploading to same deck by not importing
                //       duplicate questions
                if (!DeckAlreadyExists(deckName))
                {
                    var deck = new MochiDeck();
                    deck.Name = _deckName;
                    deck.ParentId = _selectedDeck!.Id;

                    deck = await mochiClient.CreateDeck(deck);

                    foreach (Card card in _cards.Cards)
                    {
                        var mochiCard = new MochiCard();
                        mochiCard.Content = $"{card.Question}\n---\n{card.Answer}";
                        mochiCard.DeckId = deck.Id;

                        await mochiClient.CreateCard(mochiCard);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async void GetMochiDecks()
        {
            try
            {
                var mochiClient = new MochiClient(SettingsManager.ApplicationSettings.Mochi.ApiKey);
                var deckResponse = await mochiClient.GetDeckList();
                _decks = deckResponse.Decks;

                if (_decks.Count > 0)
                {
                    _selectedDeck = _decks[0];
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
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
