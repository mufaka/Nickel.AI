using ImGuiNET;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Controls
{
    public class FileChooser : ImGuiControl
    {
        private DriveInfo[] _drives;
        private DriveInfo? _selectedDrive;
        private DirectoryInfo? _selectedDirectory;
        private FileInfo? _selectedFile;

        public FileChooser()
        {
            _drives = DriveInfo.GetDrives();
        }

        public override void Render()
        {
            foreach (DriveInfo drive in _drives)
            {
                if (ImGui.Button(drive.Name))
                {
                    _selectedDrive = drive;
                    _selectedDirectory = null;
                }
                ImGui.SameLine();
            }

            if (_selectedDrive != null)
            {
                ImGui.PushFont(UiManager.FONT_JETBRAINS_MONO_MEDIUM_16);
                ImGui.NewLine();
                ImGui.BeginChild("fc left", new Vector2(300, 0), ImGuiChildFlags.Border | ImGuiChildFlags.ResizeX | ImGuiChildFlags.AlwaysUseWindowPadding);
                ImGui.Unindent();
                RenderDirectoryTree(_selectedDrive.RootDirectory);
                ImGui.EndChild();

                if (_selectedDirectory != null)
                {
                    ImGui.SameLine();
                    ImGui.BeginGroup();
                    ImGui.BeginChild("fc right", new Vector2(0, -ImGui.GetFrameHeightWithSpacing()));
                    ImGui.SeparatorText(_selectedDirectory.FullName);

                    /*
                    var files = _selectedDirectory.GetFiles();

                    if (files.Length > 0)
                    {
                        // NOTE: Should probably enforce a max on files
                        foreach (var file in files)
                        {
                            if (ImGui.TreeNodeEx(file.Name))
                            {
                                // what to do here?
                                ImGui.TreePop();
                            }
                        }
                    }
                    */

                    ImGui.EndChild();
                    ImGui.EndGroup();
                }
                ImGui.PopFont();
            }
        }

        private void RenderDirectoryTree(DirectoryInfo directory)
        {
            try
            {
                var directories = directory.GetDirectories();

                bool nodeOpen = ImGui.TreeNodeEx(directory.Name);
                bool nodeClicked = ImGui.IsItemClicked();

                if (nodeClicked)
                {
                    _selectedDirectory = directory;
                }

                if (nodeOpen)
                {
                    foreach (var childDirectory in directories)
                    {
                        RenderDirectoryTree(childDirectory);
                    }

                    ImGui.TreePop();
                }
            }
            catch (Exception ex)
            {
                // TODO: Handle access exceptions? There doesn't seem to be a good way to check permissions
                //       across platforms ... at least that I can test. 
            }
        }
    }
}
