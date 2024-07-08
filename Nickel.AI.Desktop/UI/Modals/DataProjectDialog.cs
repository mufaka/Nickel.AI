using ImGuiNET;
using Nickel.AI.Desktop.Models;
using System.Numerics;



namespace Nickel.AI.Desktop.UI.Modals
{
    public class DataProjectDialog : IModalDialog
    {
        public DataProject Project { get; set; } = new DataProject();
        public bool OK { get; set; } = false;

        private string _projectName = string.Empty;
        private string _sourcePath = string.Empty;
        private string _destinationPath = string.Empty;
        private int _chunkSize = 1000;
        private ChooseFileDialog _sourceDialog = new ChooseFileDialog();
        private ChooseFileDialog _destinationDialog = new ChooseFileDialog();

        public void ShowDialogButton(string buttonText, string dialogLabel)
        {
            if (!String.IsNullOrWhiteSpace(_sourceDialog.SelectedPath))
            {
                _sourcePath = _sourceDialog.SelectedPath;
            }

            if (!String.IsNullOrWhiteSpace(_destinationDialog.SelectedPath))
            {
                _destinationPath = _destinationDialog.SelectedPath;
            }

            if (ImGui.Button(buttonText))
            {
                ImGui.OpenPopup(dialogLabel);

                var center = ImGui.GetMainViewport().GetCenter();
                ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
                ImGui.SetNextWindowSize(new Vector2(800, 400));
            }

            if (ImGui.BeginPopupModal(dialogLabel))
            {
                ImGui.InputText("Name", ref _projectName, 128);

                ImGui.InputText("Source", ref _sourcePath, 256);
                ImGui.SameLine();
                _sourceDialog.ShowDialogButton("File", "Choose Source File");

                ImGui.InputText("Destination", ref _destinationPath, 256);
                ImGui.SameLine();
                _destinationDialog.ShowDialogButton("Directory", "Choose Destination Directory");

                ImGui.InputInt("Chunk Size", ref _chunkSize, 1, 10);
                ImGui.SameLine();

                // TODO: Move this to a utility
                ImGui.TextDisabled("(?)");
                if (ImGui.BeginItemTooltip())
                {
                    ImGui.PushTextWrapPos(ImGui.GetFontSize() * 35.0f);
                    ImGui.TextUnformatted("CTRL-Click for faster steps.");
                    ImGui.PopTextWrapPos();
                    ImGui.EndTooltip();
                }

                // TODO: Show existing projects
                Project.Name = _projectName;
                Project.SourcePath = _sourcePath;
                Project.DestinationPath = _destinationPath;
                Project.ChunkSize = _chunkSize;

                if (Project.IsValid())
                {
                    if (ImGui.Button("OK"))
                    {
                        OK = true;
                        ImGui.CloseCurrentPopup();
                    }
                    ImGui.SameLine();
                }

                if (ImGui.Button("Cancel"))
                {
                    OK = false;
                    ImGui.CloseCurrentPopup();
                }
            }
        }
    }
}
