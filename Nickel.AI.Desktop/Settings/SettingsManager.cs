using Newtonsoft.Json;
using Nickel.AI.Desktop.Models;

namespace Nickel.AI.Desktop.Settings
{
    public static class SettingsManager
    {
        public static string SETTINGS_ROOT = "Settings";
        public static string DATA_PROJECTS = "data_projects.json";

        private static void InitializeSettingsDirectory()
        {
            if (!Directory.Exists(SETTINGS_ROOT))
            {
                Directory.CreateDirectory(SETTINGS_ROOT);
            }
        }

        private static List<DataProject> _dataProjects = new List<DataProject>();

        public static List<DataProject> DataProjects
        {
            get
            {
                // see if there are any to load
                if (_dataProjects.Count == 0)
                {
                    var path = Path.Combine(SETTINGS_ROOT, DATA_PROJECTS);
                    if (File.Exists(path))
                    {
                        var projects = JsonConvert.DeserializeObject<List<DataProject>>(File.ReadAllText(path));

                        if (projects != null)
                        {
                            _dataProjects = projects;
                        }
                    }
                }

                return _dataProjects;
            }
            set
            {
                InitializeSettingsDirectory();
                var path = Path.Combine(SETTINGS_ROOT, DATA_PROJECTS);

                _dataProjects = value;
                File.WriteAllText(path, JsonConvert.SerializeObject(value));
            }
        }
    }
}
