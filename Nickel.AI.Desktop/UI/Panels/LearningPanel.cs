using Hexa.NET.ImGui;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.External.Mochi;
using Nickel.AI.Desktop.Models;
using Nickel.AI.Desktop.Settings;
using Nickel.AI.Desktop.UI.Modals;
using Nickel.AI.Embeddings;
using Nickel.AI.VectorDB;
using OllamaSharp;
using OllamaSharp.Models;
using System.Collections.Concurrent;
using System.Text;
using TesseractOCR;
using TesseractOCR.Enums;

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

        private ChooseFileDialog _sourceDialog = new ChooseFileDialog();
        private string _imageDir = string.Empty;
        private string _collectionName = string.Empty;

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
                if (!String.IsNullOrWhiteSpace(_sourceDialog.SelectedPath))
                {
                    _imageDir = _sourceDialog.SelectedPath;
                }


                ImGui.InputText("Subject", ref _subject, 256, ImGuiInputTextFlags.None);
                ImGui.InputText("Topic", ref _topic, 256, ImGuiInputTextFlags.None);
                ImGui.InputText("Image Dir", ref _imageDir, 256, ImGuiInputTextFlags.None);
                ImGui.SameLine();
                _sourceDialog.ShowDialogButton("...", "Choose Dir");
                ImGui.InputText("Collection Name", ref _collectionName, 256, ImGuiInputTextFlags.None);

                if (!String.IsNullOrWhiteSpace(_subject) && !String.IsNullOrWhiteSpace(_topic))
                {

                    if (ImGui.Button("Create Cards"))
                    {
                        _selectedCardIndex = 0;
                        _cardSide = 0;
                        _fetchingDecks = false;
                        CreateCards();
                    }

                    ImGui.SameLine();

                    if (!String.IsNullOrWhiteSpace(_imageDir))
                    {
                        if (ImGui.Button("Create Cards From Images"))
                        {
                            _selectedCardIndex = 0;
                            _cardSide = 0;
                            _fetchingDecks = false;
                            CreateCardsFromImageFiles();
                        }

                        ImGui.SameLine();

                        if (ImGui.Button("Create Notes From Images"))
                        {
                            CreateNotesFromImageFiles();
                        }
                    }
                }

                if (!String.IsNullOrEmpty(_collectionName))
                {
                    if (ImGui.Button("Index Text From Images"))
                    {
                        IndexTextFromImageFiles();
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

        private void NextCard()
        {
            int newIndex = _selectedCardIndex;

            while (true)
            {
                newIndex--;

                if (newIndex < 0)
                {
                    newIndex = _cards.Cards.Count - 1;
                }

                if (!_cards.Cards[newIndex].Know)
                {
                    _selectedCardIndex = newIndex;
                }

                if (newIndex == _selectedCardIndex)
                {
                    break;
                }
            }
        }

        private void PreviousCard()
        {
            int newIndex = _selectedCardIndex;

            while (true)
            {
                newIndex++;

                if (newIndex >= _cards.Cards.Count)
                {
                    newIndex = 0;
                }

                if (!_cards.Cards[newIndex].Know)
                {
                    _cardSide = 0;
                    _selectedCardIndex = newIndex;
                }

                if (newIndex == _selectedCardIndex)
                {
                    break;
                }
            }
        }

        /// <summary>
        /// Card Stats
        /// </summary>
        /// <returns>know, total</returns>
        private (int, int) GetCardStats()
        {
            return (_cards.Cards.Where(c => c.Know).Count(), _cards.Cards.Count);
        }

        private void RenderCards()
        {
            if (_selectedCardIndex >= 0)
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
                    PreviousCard();
                }

                ImGui.SameLine();

                if (ImGui.Button("Know"))
                {
                    card.Know = true;
                    NextCard();
                }

                ImGui.SameLine();

                if (ImGui.Button("Next"))
                {
                    NextCard();
                }

                ImGui.SameLine();

                if (ImGui.Button("Detail"))
                {
                    MessageQueue.Instance.Enqueue(UiMessageConstants.CHAT_ASK_QUESTION, card.Question);
                }
            }

            var stats = GetCardStats();

            ImGui.Text($"You know {stats.Item1} of {stats.Item2} facts.");
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
            var existing = _decks?.Where(d => d.Name == name).FirstOrDefault();
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

        private async Task<string> GetChatResponse(string prompt)
        {
            var ollamaEndpointUrl = SettingsManager.ApplicationSettings.Ollama.EndPoint;

            if (String.IsNullOrWhiteSpace(ollamaEndpointUrl))
            {
                _logger.LogInformation("Ollama endpoint is not configured. Using \"http://localhost:11434\" as a default.");
                ollamaEndpointUrl = "http://localhost:11434";
                SettingsManager.ApplicationSettings.Ollama.EndPoint = ollamaEndpointUrl;
            }

            Uri? ollamaEndpoint;

            if (!Uri.TryCreate(ollamaEndpointUrl, UriKind.Absolute, out ollamaEndpoint))
            {
                _logger.LogInformation($"Ollama configured endpoint is invalid [{ollamaEndpointUrl}]. Using \"http://localhost:11434\" as a default.");
                ollamaEndpoint = new Uri("http://localhost:11434");
                SettingsManager.ApplicationSettings.Ollama.EndPoint = "http://localhost:11434";
            }

            var model = SettingsManager.ApplicationSettings.Ollama.Model;

            if (String.IsNullOrWhiteSpace(model))
            {
                _logger.LogInformation("Ollama model is not configured. Using \"llama3.1\" as a default.");
                model = "llama3.1";
                SettingsManager.ApplicationSettings.Ollama.Model = model;
            }

            var ollama = new OllamaApiClient(ollamaEndpoint);

            var completionRequest = new GenerateCompletionRequest();
            completionRequest.Stream = false;

            var requestOptions = new RequestOptions()
            {
                Temperature = 0.3f,
            };

            completionRequest.Options = requestOptions;
            completionRequest.Prompt = prompt;
            completionRequest.Model = model;

            var completionResponse = await ollama.GetCompletion(completionRequest);
            return completionResponse.Response;
        }

        private List<string> ReadImageFiles(string directory, bool splitByBlock)
        {
            var files = Directory.GetFiles(directory);
            var bag = new ConcurrentBag<string>();

            _logger.LogInformation("Processing {file_count} files in {directory}.", files.Length, directory);

            Parallel.ForEach(files, (file) =>
            {
                try
                {
                    // NOTE: The engine only allows for processing one image at a time so we are
                    //       creating a new instance for each file.
                    using var engine = new Engine(@"./TesseractData", Language.English, EngineMode.Default);
                    using var img = TesseractOCR.Pix.Image.LoadFromFile(file);
                    using var page = engine.Process(img);

                    var buff = new StringBuilder();

                    foreach (var block in page.Layout)
                    {
                        foreach (var paragraph in block.Paragraphs)
                        {
                            foreach (var textLine in paragraph.TextLines)
                            {
                                if (textLine.Confidence > 65.0 && !String.IsNullOrWhiteSpace(textLine.Text))
                                {
                                    buff.Append(textLine.Text);
                                }
                            }
                        }

                        if (splitByBlock && buff.Length > 0)
                        {
                            bag.Add(buff.ToString());
                            buff.Clear();
                        }
                    }

                    if (!splitByBlock && buff.Length > 0)
                    {
                        bag.Add(buff.ToString());
                        buff.Clear();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogWarning("Unable to read text from {file_name}. {message}", file, ex.Message);
                }
            });

            _logger.LogInformation("Created {text_count} texts from images.", bag.Count);
            return bag.ToList();
        }

        private async void IndexTextFromImageFiles()
        {
            var directory = _imageDir;
            //var embedder = new OllamaEmbedder("http://localhost:11434", "llama3.1");
            var embedder = new OllamaEmbedder("http://localhost:11434", "bge-large");
            var texts = ReadImageFiles(directory, true);

            // TODO: This needs to be a UI input
            IVectorDB qdrant = new QdrantVectorDB("http://localhost:6334");

            await CreateKnowledgebase(_collectionName, qdrant, embedder, texts);
        }

        private static async Task CreateKnowledgebase(string collectionName, IVectorDB qdrant, OllamaEmbedder embedder, List<string> documents)
        {
            // TODO: double check the size parameter. Should match embedding vector size
            var created = await qdrant.CreateCollection(collectionName, 1024, DistanceType.Cosine);

            if (created)
            {
                // create vector points from documents
                List<VectorPoint> points = new List<VectorPoint>();

                foreach (var document in documents)
                {
                    var point = new VectorPoint();

                    // TODO: Need to come up with an ID scheme that lets this operation be idempotent
                    point.Id = Guid.NewGuid().ToString();

                    // TODO: embedder uses double[], qdrant client uses float[]. Casting here for now but this sucks because it
                    //       could barf (64 bit to 32 bit ..)
                    var embeddings = await embedder.GetEmbedding(document);
                    point.Vectors = embeddings.Select(f => (float)f).ToArray();
                    point.Payload = new Dictionary<string, string>()
                {
                    { "document", document }
                };

                    if (point.Vectors.Length > 0)
                    {
                        points.Add(point);
                    }
                }

                // TODO: Should this be paged?
                qdrant.Upsert(collectionName, points);
            }
        }

        private async void CreateNotesFromImageFiles()
        {
            var flashCards = new FlashCards();
            var directory = _imageDir;
            var files = Directory.GetFiles(directory);
            var texts = ReadImageFiles(directory, false);

            foreach (var text in texts)
            {
                var prompt = @$"Summarize the given text for someone studying {_subject} {_topic}

text:
{text}
";
                var response = await GetChatResponse(prompt);

                var card = new Card()
                {
                    Question = response,
                    Detail = string.Empty
                };

                flashCards.Cards.Add(card);
            }

            _logger.LogInformation("Created {card_count} notes from images", flashCards.Cards.Count);
            _cards = flashCards;
        }

        private async void CreateCardsFromImageFiles()
        {
            var flashCards = new FlashCards();
            var directory = _imageDir;
            var files = Directory.GetFiles(directory);
            var texts = ReadImageFiles(directory, false);

            foreach (var text in texts)
            {
                var prompt = @$"I am a student learning {_subject}. Generate flash cards for me covering {_topic} using the provided context. Respond in json with the following format:

        {{
            ""cards"": [
                {{ ""question"": """", ""answer"": """" }}  
            ]
        }}

context:
{text}
";
                var response = await GetChatResponse(prompt);
                flashCards.AddCardsFromLlmResponse(response);

                _logger.LogInformation("{0} cards created for subject {0}, topic {1}", flashCards.Cards.Count, _subject, _topic);
            }

            _cards = flashCards;
        }

        private async void CreateCards()
        {
            try
            {
                if (_subject.Length > 0 && _topic.Length > 0)
                {
                    try
                    {
                        var prompt = @$"I am a student learning {_subject}. Generate 30 flash cards for me covering {_topic}. Respond in json with the following format:

{{
    ""cards"": [
        {{ ""question"": """", ""answer"": """" }}  
    ]
}}
";

                        var response = await GetChatResponse(prompt);
                        _cards = FlashCards.FromLlmResponse(response);
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
