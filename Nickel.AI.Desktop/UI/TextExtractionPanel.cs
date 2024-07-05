using ImGuiNET;
using Nickel.AI.Desktop.UI.Modals;

namespace Nickel.AI.Desktop.UI
{
    public class TextExtractionPanel : Panel
    {
        private ChooseFileDialog _fileDialog = new ChooseFileDialog();
        private string _selectedPath = String.Empty;

        public override void DoRender()
        {
            ImGui.Text("Path (File or Url)");
            ImGui.SameLine();
            ImGui.InputText("", ref _selectedPath, (uint)256);
            ImGui.SameLine();
            _fileDialog.ShowDialogButton("...", "Choose File");

            if (_fileDialog.SelectedPath != String.Empty && File.Exists(_fileDialog.SelectedPath))
            {
                _selectedPath = _fileDialog.SelectedPath;
            }
            else
            {
                _selectedPath = String.Empty;
            }

            // TODO: Add radios for which Chunker to use (or none).

            // TODO: Add "Extract" button.

            // TODO: Layout

            // Text extraction can take either a file path or URL and it will "do the right thing".

            // TODO: Need to have some way of showing errors. Popup? Add a console log panel as well.
        }

        public override void Update()
        {

        }
    }
}
