using Hexa.NET.ImGui;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.Settings;
using Nickel.AI.Embeddings;
using Nickel.AI.VectorDB;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class VectorDbPanel : Panel
    {
        private ILogger _logger;
        private string _qdrantUrl;
        private string _ollamaUrl;
        private IVectorDB? _qdrant;
        private IEmbedder? _embedder;
        private bool _inErrorState = false;
        private List<string>? _collections;
        private string _selectedCollection = string.Empty;
        private string _searchQuery = string.Empty;
        private List<VectorPoint>? _searchResults;

        public VectorDbPanel(ILogger<VectorDbPanel> logger)
        {
            _logger = logger;
            _qdrantUrl = SettingsManager.ApplicationSettings.Qdrant.EndPoint;
            _ollamaUrl = SettingsManager.ApplicationSettings.Ollama.EndPoint;

            if (String.IsNullOrWhiteSpace(_qdrantUrl))
            {
                _logger.LogInformation("Qdrant endpoint is not configured. Using \"http://localhost:6334\" as a default.");
                _qdrantUrl = "http://localhost:6334";
            }

            if (String.IsNullOrWhiteSpace(_ollamaUrl))
            {
                _logger.LogInformation("Ollama endpoint is not configured. Using \"http://localhost:11434\" as a default.");
                _ollamaUrl = "http://localhost:11434";
            }

            _embedder = new OllamaEmbedder(_ollamaUrl, "bge-large");

            InitializeCollections();
        }

        public override void HandleUiMessage(UiMessage message)
        {
        }

        private void InitializeCollections()
        {
            try
            {
                if (Uri.TryCreate(_qdrantUrl, UriKind.Absolute, out var _)) // NOTE: Don't need the Uri, just validating the url
                {
                    SettingsManager.ApplicationSettings.Qdrant.EndPoint = _qdrantUrl;
                    _qdrant = new QdrantVectorDB(_qdrantUrl);
                }

                if (_qdrant != null && _collections == null)
                {
                    _collections = _qdrant.ListCollections().Result;

                    if (_collections.Count > 0)
                    {
                        _selectedCollection = _collections[0];

                        foreach (string collection in _collections)
                        {
                            _logger.LogInformation($"Found Qdrant collection: {collection}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _inErrorState = true;
            }
        }

        public override void DoRender()
        {
            try
            {
                if (ImGui.InputText("Qdrant Url", ref _qdrantUrl, 256, ImGuiInputTextFlags.EnterReturnsTrue))
                {
                    _collections = null;
                    InitializeCollections();
                }

                if (!_inErrorState && _collections != null)
                {
                    var availableRegion = ImGui.GetContentRegionAvail();

                    if (ImGui.BeginChild("OptionsChild", new Vector2(availableRegion.X, ImGui.GetTextLineHeightWithSpacing() * 2), ImGuiChildFlags.Border | ImGuiChildFlags.AutoResizeY))
                    {
                        if (ImGui.BeginCombo("##collections", _selectedCollection, ImGuiComboFlags.WidthFitPreview))
                        {
                            foreach (string collection in _collections)
                            {
                                var isSelected = collection == _selectedCollection;

                                if (ImGui.Selectable(collection, isSelected))
                                {
                                    _selectedCollection = collection;
                                }

                                if (isSelected)
                                {
                                    ImGui.SetItemDefaultFocus();
                                }
                            }

                            ImGui.EndCombo();
                        }

                        ImGui.SameLine();

                        if (ImGui.InputText("Search", ref _searchQuery, 256, ImGuiInputTextFlags.EnterReturnsTrue))
                        {
                            // TODO: we need to know the type of embeddings that were used.

                            var searchVector = _embedder!.GetEmbedding(_searchQuery).Result.Select(f => (float)f).ToArray();

                            _searchResults = _qdrant!.Search(_selectedCollection, searchVector, 5).Result;
                        }

                        ImGui.EndChild();
                    }

                    if (_searchResults != null && _searchResults.Count > 0)
                    {
                        if (ImGui.BeginTabBar("Results", ImGuiTabBarFlags.None))
                        {
                            int resultCounter = 1;
                            foreach (VectorPoint point in _searchResults)
                            {
                                if (ImGui.BeginTabItem($"Result {resultCounter} ({point.Score})"))
                                {
                                    if (point.Payload != null)
                                    {
                                        ImGui.TextWrapped(string.Join(Environment.NewLine, point.Payload));
                                    }
                                    ImGui.EndTabItem();
                                }
                                resultCounter++;
                            }

                            ImGui.EndTabBar();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                _inErrorState = true;
            }
        }
    }
}
