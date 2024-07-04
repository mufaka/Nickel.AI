using ImGuiNET;
using Nickel.AI.Desktop.UI.Controls;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Modals
{
    public class ChooseFileDialog
    {
        private FileChooser _fileChooser = new FileChooser();

        public string SelectedPath { get; set; } = String.Empty;

        // TODO: Expose selected path as property

        public void ShowDialogButton(string buttonText, string dialogLabel)
        {
            if (ImGui.Button(buttonText))
            {
                ImGui.OpenPopup(dialogLabel);

                var center = ImGui.GetMainViewport().GetCenter();
                ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
                ImGui.SetNextWindowSize(new Vector2(600, 400));
            }

            if (ImGui.BeginPopupModal(dialogLabel))
            {
                //ImGui.BeginGroup();
                _fileChooser.Render();

                if (_fileChooser.SelectedFile != null)
                {
                    if (ImGui.Button("File"))
                    {
                        // TODO: set selected file
                        ImGui.CloseCurrentPopup();
                    }
                    ImGui.SameLine();
                }

                if (_fileChooser.SelectedDirectory != null)
                {
                    if (ImGui.Button("Directory"))
                    {
                        // TODO: set selected directory
                        ImGui.CloseCurrentPopup();
                    }
                    ImGui.SameLine();
                }

                if (ImGui.Button("Cancel"))
                {
                    ImGui.CloseCurrentPopup();
                }
                //ImGui.EndGroup();
                ImGui.EndPopup();
            }
        }
    }
}
