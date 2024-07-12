using ImGuiNET;
using Microsoft.Extensions.DependencyInjection;
using Nickel.AI.Desktop.UI.Controls;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Modals
{
    public class ChooseFileDialog : IModalDialog
    {
        private FileChooser _fileChooser = App.Host.Services.GetRequiredService<FileChooser>();

        public string SelectedPath { get; set; } = String.Empty;

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
                _fileChooser.Render();

                if (_fileChooser.SelectedFile != null)
                {
                    if (ImGui.Button("File"))
                    {
                        SelectedPath = _fileChooser.SelectedFile.FullName;
                        ImGui.CloseCurrentPopup();
                    }
                    ImGui.SameLine();
                }

                if (_fileChooser.SelectedDirectory != null)
                {
                    if (ImGui.Button("Directory"))
                    {
                        SelectedPath = _fileChooser.SelectedDirectory.FullName;
                        ImGui.CloseCurrentPopup();
                    }
                    ImGui.SameLine();
                }

                if (ImGui.Button("Cancel"))
                {
                    ImGui.CloseCurrentPopup();
                }

                ImGui.EndPopup();
            }
        }
    }
}
