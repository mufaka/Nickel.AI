using ImGuiNET;
using Nickel.AI.Desktop.Models;
using Nickel.AI.Desktop.Settings;
using Nickel.AI.Desktop.UI.Modals;
using System.Numerics;

namespace Nickel.AI.Desktop.UI.Panels
{
    public class ChunkedDataPanel : Panel
    {
        private DataProject? _dataProject = null;
        private DataProjectDialog _dataProjectDialog = new DataProjectDialog();
        private List<DataProject> _projects;
        private bool _showDataProjectModal = false;

        public ChunkedDataPanel()
        {
            _projects = SettingsManager.DataProjects;
            HasMenuBar = true;
        }

        public override void DoRender()
        {
            DrawMenu();
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
                }

                ImGui.EndMenuBar();
            }
        }

        public override void Update()
        {

        }
    }
}
