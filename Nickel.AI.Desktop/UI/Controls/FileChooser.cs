using ImGuiNET;

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
                }
                ImGui.SameLine();
            }

            if (_selectedDrive != null)
            {
                var directories = _selectedDrive.RootDirectory.GetDirectories();

                ImGui.NewLine();
                RenderDirectoryTree(_selectedDrive.RootDirectory);
            }

            if (_selectedDirectory != null)
            {

            }
        }

        private void RenderDirectoryTree(DirectoryInfo directory)
        {
            // TODO: Figure out if the return bool means open
            if (ImGui.TreeNodeEx(directory.Name))
            {
                ImGui.Indent();
                // are there more directories?
                var directories = directory.GetDirectories();

                foreach (var childDirectory in directories)
                {
                    RenderDirectoryTree(childDirectory);
                }

                ImGui.TreePop();
                ImGui.Unindent();
            }
        }
    }
}
