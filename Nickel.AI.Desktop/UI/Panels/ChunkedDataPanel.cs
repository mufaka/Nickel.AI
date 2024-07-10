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

        public ChunkedDataPanel()
        {
            _projects = SettingsManager.DataProjects;
            HasMenuBar = true;
        }

        public override void DoRender()
        {
            DrawMenu();

            if (_dataProject != null && _chunkedData == null)
            {
                InitializeDataProject();
            }

            if (_chunkedData != null)
            {
                // need to page the data frames
                if (_dataFrameTable.Frame == null)
                {
                    _dataFrameTable.Frame = _chunkedData.Frames[0].Data;
                }
                _dataFrameTable.Render();
            }
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
