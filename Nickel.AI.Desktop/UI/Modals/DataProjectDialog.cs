using Hexa.NET.ImGui;
using Nickel.AI.Desktop.Models;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Modals
{
    public class DataProjectDialog : IModalDialog
    {
        public DataProject Project { get; set; } = new DataProject();
        public bool OK { get; set; } = false;
        public bool Cancel { get; set; } = false;

        private string _projectName = string.Empty;
        private string _sourcePath = string.Empty;
        private string _destinationPath = string.Empty;
        private int _frameSize = 1000;
        private ChooseFileDialog _sourceDialog = new ChooseFileDialog();
        private ChooseFileDialog _destinationDialog = new ChooseFileDialog();

        public void ShowDialogButton(string buttonText, string dialogLabel)
        {
            var buttonClicked = ImGui.Button(buttonText);

            if (buttonClicked)
            {
                ImGui.OpenPopup(dialogLabel);

                //var center = ImGui.GetMainViewport().GetCenter();
                var center = ImGui.GetWorkCenter(ImGui.GetMainViewport());
                ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
                ImGui.SetNextWindowSize(new Vector2(800, 400));
            }

            ShowModal(dialogLabel);

        }

        public void ShowModal(string dialogLabel)
        {
            OK = false;
            Cancel = false;

            if (!String.IsNullOrWhiteSpace(_sourceDialog.SelectedPath))
            {
                _sourcePath = _sourceDialog.SelectedPath;
            }

            if (!String.IsNullOrWhiteSpace(_destinationDialog.SelectedPath))
            {
                _destinationPath = _destinationDialog.SelectedPath;
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

                ImGui.InputInt("Chunk Size", ref _frameSize, 1, 10);
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
                Project.FrameSize = _frameSize;

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
                    Cancel = true;
                    ImGui.CloseCurrentPopup();
                }
            }
        }
    }
}
