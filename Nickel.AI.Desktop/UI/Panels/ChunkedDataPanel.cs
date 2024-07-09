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
        }

        public override void DoRender()
        {
            // draw menu for window
            if (ImGui.BeginMenu("Data Projects"))
            {
                ImGui.MenuItem("<New>", _showDataProjectModal);
            }

            if (_showDataProjectModal)
            {
                ImGui.OpenPopup("Testing Menu Click Dialog");

                var center = ImGui.GetMainViewport().GetCenter();
                ImGui.SetNextWindowPos(center, ImGuiCond.Appearing, new Vector2(0.5f, 0.5f));
                ImGui.SetNextWindowSize(new Vector2(800, 400));
            }
            _dataProjectDialog.ShowModal("Testing Menu Click Dialog");

            // choose existing project or create new. 

            // NOTE: what about editing existing? Keep as TODO..
        }

        public override void Update()
        {

        }
    }
}
