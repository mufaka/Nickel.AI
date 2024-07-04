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

                    var files = _selectedDirectory.GetFiles();

                    if (files.Length > 0)
                    {
                        if (ImGui.BeginTable("files", 2, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
                        {
                            ImGui.TableSetupColumn("File Name");
                            ImGui.TableSetupColumn("Size");
                            ImGui.TableHeadersRow();

                            foreach (var file in files)
                            {
                                ImGui.TableNextRow();

                                ImGui.TableSetColumnIndex(0);
                                ImGui.Text(file.Name);

                                ImGui.TableSetColumnIndex(1);
                                ImGui.Text(SizeSuffix(file.Length));
                            }

                            ImGui.EndTable();
                        }
                    }

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

        // size formatting copied from https://stackoverflow.com/questions/14488796/does-net-provide-an-easy-way-convert-bytes-to-kb-mb-gb-etc
        static readonly string[] SizeSuffixes =
                          { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        static string SizeSuffix(Int64 value, int decimalPlaces = 1)
        {
            if (value < 0) { return "-" + SizeSuffix(-value, decimalPlaces); }

            int i = 0;
            decimal dValue = (decimal)value;
            while (Math.Round(dValue, decimalPlaces) >= 1000)
            {
                dValue /= 1024;
                i++;
            }

            return string.Format("{0:n" + decimalPlaces + "} {1}", dValue, SizeSuffixes[i]);
        }
    }
}
