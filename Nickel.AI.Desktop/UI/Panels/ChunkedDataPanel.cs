using ImGuiNET;
using Nickel.AI.Data;
using Nickel.AI.Desktop.Models;
using Nickel.AI.Desktop.Settings;
using Nickel.AI.Desktop.UI.Controls;
using Nickel.AI.Desktop.UI.Modals;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class ChunkedDataPanel : Panel
    {
        private DataProject? _dataProject = null;
        private DataProjectDialog _dataProjectDialog = new DataProjectDialog();
        private List<DataProject> _projects;
        private ChunkedData? _chunkedData;
        private DataFrameTable _dataFrameTable = new DataFrameTable();

        private bool _showDataProjectModal = false;
        private bool _creatingProject = false;

        private int _frameNumber = 1;

        public ChunkedDataPanel()
        {
            _projects = SettingsManager.DataProjects;
            HasMenuBar = true;
        }

        public override void DoRender()
        {
            DrawMenu();

            if (_dataProject != null)
            {
                DrawProjectDetails();

                if (_chunkedData == null)
                {
                    InitializeDataProject();
                }
            }

            if (_chunkedData != null)
            {
                if (_chunkedData.Frames.Count > 0)
                {
                    _dataFrameTable.Frame = _chunkedData.Frames[_frameNumber - 1].Data;

                    if (ImGui.BeginCombo("Choose Frame", $"Frame {_frameNumber}", ImGuiComboFlags.WidthFitPreview))
                    {
                        for (int x = 0; x < _chunkedData.Frames.Count; x++)
                        {
                            var isSelected = _frameNumber == x + 1;

                            if (ImGui.Selectable($"Frame {x + 1}", isSelected))
                            {
                                _dataFrameTable.PageNumber = 1;
                                _frameNumber = x + 1;
                            }

                            if (isSelected)
                            {
                                ImGui.SetItemDefaultFocus();
                            }
                        }

                        ImGui.EndCombo();
                    }

                    ImGui.Separator();

                    _dataFrameTable.Render();
                }
            }
        }

        private void DrawProjectDetails()
        {
            ImGui.SameLine(ImGui.GetContentRegionAvail().X * .5f);
            ImGui.PushStyleVar(ImGuiStyleVar.ChildRounding, 5.0f);
            ImGui.BeginChild("project-detail", new Vector2(ImGui.GetContentRegionAvail().X, 100), ImGuiChildFlags.Border, ImGuiWindowFlags.HorizontalScrollbar);
            ImGui.BeginTable("project-table", 2);
            ImGui.TableSetupColumn("Attribute", ImGuiTableColumnFlags.WidthFixed, 100.0f);

            ImGui.PushFont(UiManager.FONT_JETBRAINS_MONO_MEDIUM_16);
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Text("Project");
            ImGui.TableNextColumn();
            ImGui.Text(_dataProject!.Name);

            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Text("Source");
            ImGui.TableNextColumn();
            ImGui.Text(_dataProject!.SourcePath);

            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Text("Storage");
            ImGui.TableNextColumn();
            ImGui.Text(_dataProject!.DestinationPath);

            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            ImGui.Text("Chunk Size");
            ImGui.TableNextColumn();
            ImGui.Text(Convert.ToString(_dataProject!.ChunkSize));
            ImGui.PopFont();

            ImGui.EndTable();
            ImGui.EndChild();
            ImGui.PopStyleVar();
        }

        private void DrawMenu()
        {
            if (ImGui.BeginMenuBar())
            {
                // draw menu for window
                if (ImGui.BeginMenu("Data Projects"))
                {
                    _showDataProjectModal = ImGui.MenuItem("<New>");

                    if (_projects.Count > 0)
                    {
                        ImGui.Separator();

                        foreach (DataProject project in _projects)
                        {
                            if (ImGui.MenuItem(project.Name))
                            {
                                _frameNumber = 1;
                                _chunkedData = null;
                                _dataFrameTable.Frame = null;
                                _dataProject = project;
                            }
                        }
                    }
                    ImGui.EndMenu();
                }

                if (_showDataProjectModal)
                {
                    // NOTE: Clearing these out because the modal appears under a loaded project for some reason
                    //       and this is the easiest way to handle that.
                    _frameNumber = 1;
                    _chunkedData = null;
                    _dataFrameTable.Frame = null;
                    _dataProject = null;

                    ImGui.OpenPopup("Testing Menu Click Dialog");

                    var center = ImGui.GetMainViewport().GetCenter();
                    ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
                    ImGui.SetNextWindowSize(new Vector2(800, 400));
                }

                _dataProjectDialog.ShowModal("Testing Menu Click Dialog");

                if (_dataProjectDialog.OK || _dataProjectDialog.Cancel)
                {
                    _showDataProjectModal = false;

                    if (_dataProjectDialog.OK)
                    {
                        CreateNewProject(_dataProjectDialog.Project);
                    }
                }

                ImGui.EndMenuBar();
            }
        }

        private void CreateNewProject(DataProject project)
        {
            SettingsManager.DataProjects.Add(project);
            // TODO: This is janky. Have an explicit save instead?
            SettingsManager.DataProjects = SettingsManager.DataProjects;

            _dataProject = project;
        }

        private void InitializeDataProject()
        {
            // TODO: This needs to be async
            try
            {
                var loader = new CsvDataLoader(_dataProject!.SourcePath, _dataProject!.ChunkSize, true);
                var storage = new CsvDataFrameStorage(_dataProject.DestinationPath);

                ChunkedData chunkedData = new ChunkedData();

                // TODO: Initialization check should check for metadata file and compare chunk size in addition to the following.
                if (!Directory.Exists(_dataProject.DestinationPath) || Directory.GetFiles(_dataProject.DestinationPath).Length == 0)
                {
                    Directory.CreateDirectory(_dataProject.DestinationPath);
                    chunkedData.Initialize(loader, storage);
                }
                else
                {
                    chunkedData.Load(storage);
                }

                _chunkedData = chunkedData;
            }
            catch (Exception ex)
            {
                // TODO: Logging
            }
        }


        public override void Update()
        {

        }
    }
}
