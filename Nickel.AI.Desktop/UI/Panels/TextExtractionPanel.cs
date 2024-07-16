using ImGuiNET;
using Microsoft.Extensions.Logging;
using Nickel.AI.Chunking;
using Nickel.AI.Desktop.UI.Modals;
using Nickel.AI.Desktop.Utilities;
using Nickel.AI.Extraction;
using Nickel.AI.Tokenization;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class TextExtractionPanel : Panel
    {
        private ChooseFileDialog _fileDialog = new ChooseFileDialog();
        private string _selectedPath = string.Empty;
        private ExtractedDocument? _extractedDocument;
        private readonly ILogger _logger;

        public TextExtractionPanel(ILogger<TextExtractionPanel> logger)
        {
            _logger = logger;
        }

        public override void HandleUiMessage(UiMessage message)
        {

        }

        public override void DoRender()
        {
            float windowWidth = ImGui.GetWindowWidth();
            float characterWidth = ImGui.CalcTextSize("#").X;

            ImGui.Text("Path (File or Url)");
            ImGui.SameLine();

            ImGui.InputText("", ref _selectedPath, 256, ImGuiInputTextFlags.EnterReturnsTrue);


            ImGui.SameLine();
            _fileDialog.ShowDialogButton("...", "Choose File");

            if (_fileDialog.SelectedPath != string.Empty && File.Exists(_fileDialog.SelectedPath))
            {
                _selectedPath = _fileDialog.SelectedPath;
            }

            // TODO: Add radios for which Chunker to use (or none).

            // TODO: Add "Extract" button.
            if (!string.IsNullOrWhiteSpace(_selectedPath) && ImGui.Button("Extract"))
            {
                _extractedDocument = null;
                ExtractDocument(_selectedPath);
            }

            if (_extractedDocument != null)
            {
                if (ImGui.TreeNodeEx("Paragraphs"))
                {
                    float inputWidth = windowWidth - 70;
                    for (int i = 0; i < _extractedDocument.Paragraphs.Count; i++)
                    {
                        var paragraph = TextUtilities.WordWrap(_extractedDocument.Paragraphs[i], characterWidth, inputWidth);

                        if (ImGui.TreeNodeEx($"Paragraph {i + 1}"))
                        {
                            ImGui.InputTextMultiline($"##Para{i + 1}", ref paragraph, (uint)paragraph.Length, new System.Numerics.Vector2(inputWidth, 300));

                            ImGui.TreePop();
                        }
                    }
                }
            }
        }

        private void ExtractDocument(string selectedPath)
        {
            var thread = new Thread(() => { ExtractDocumentAsync(selectedPath); });
            thread.Start();
        }

        private void ExtractDocumentAsync(string selectedPath)
        {
            _logger.LogInformation($"Started extracting text from {selectedPath}");
            // TODO: Target Token count and overlap factor should be options
            var chunker = new SemanticKernelTextChunker(500);
            var tokenizer = new TiktokenTokenizer();

            var extractor = new TextExtractor(chunker, tokenizer);

            try
            {
                _extractedDocument = extractor.Extract(new Uri(selectedPath));

                _logger.LogInformation($"Finished extracting text from {selectedPath}");
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, ex.Message);
            }
        }
    }
}
