using Hexa.NET.ImGui;
using Microsoft.Extensions.Logging;
using Nickel.AI.Desktop.Settings;
using Nickel.AI.VectorDB;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class VectorDbPanel : Panel
    {
        private ILogger _logger;
        private string _qdrantUrl;
        IVectorDB? _qdrant;
        private bool _inErrorState = false;
        private List<string>? _collections;
        private string _selectedCollection = string.Empty;

        public VectorDbPanel(ILogger<VectorDbPanel> logger)
        {
            _logger = logger;
            _qdrantUrl = SettingsManager.ApplicationSettings.Qdrant.EndPoint;

            if (String.IsNullOrWhiteSpace(_qdrantUrl))
            {
                _logger.LogInformation("Qdrant endpoint is not configured. Using \"http://localhost:6334\" as a default.");
                _qdrantUrl = "http://localhost:6334";
            }

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
