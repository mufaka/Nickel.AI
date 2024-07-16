using Newtonsoft.Json;
using Nickel.AI.Desktop.Models;

namespace Nickel.AI.Desktop.Settings
{
    public static class SettingsManager
    {
        public static string SETTINGS_ROOT = "Settings";
        public static string DATA_PROJECTS = "data_projects.json";
        public static string APP_SETTINGS = "app_settings.json";

        private static ApplicationSettings? _applicationSettings = null;
        private static List<DataProject> _dataProjects = new List<DataProject>();


        private static void InitializeSettingsDirectory()
        {
            if (!Directory.Exists(SETTINGS_ROOT))
            {
                Directory.CreateDirectory(SETTINGS_ROOT);
            }
        }

        public static void SaveAll()
        {
            if (_applicationSettings != null)
            {
                ApplicationSettings = _applicationSettings;
            }

            if (_dataProjects != null)
            {
                DataProjects = _dataProjects;
            }
        }

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

        public static ApplicationSettings ApplicationSettings
        {
            get
            {
                if (_applicationSettings == null)
                {
                    var path = Path.Combine(SETTINGS_ROOT, APP_SETTINGS);

                    if (File.Exists(path))
                    {
                        _applicationSettings = JsonConvert.DeserializeObject<ApplicationSettings>(File.ReadAllText(path));
                    }
                    else
                    {
                        _applicationSettings = new ApplicationSettings();
                    }
                }

                // NOTE: Not possible to be null with above check
#pragma warning disable CS8603 // Possible null reference return.
                return _applicationSettings;
#pragma warning restore CS8603 // Possible null reference return.
            }

            set
            {
                InitializeSettingsDirectory();
                var path = Path.Combine(SETTINGS_ROOT, APP_SETTINGS);
                File.WriteAllText(path, JsonConvert.SerializeObject(value, Formatting.Indented));
            }
        }
    }
}
