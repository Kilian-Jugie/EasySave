using EasySave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EasySaveController {
    /// <inheritdoc cref="IEasySaveController"/>
    public class EasySaveController : IEasySaveController {
        public const string PATH_LANGS = "../langs";
        public const string PATH_PROJECTS = "../projects";
        public const string PATH_LOGS = "../logs";
        public const string PATH_SAVES_LOGS = PATH_LOGS + "/saves-logs";
        public const string PATH_STATE_LOGS = PATH_LOGS + "/state-logs.log";
        public const string FILE_DEBUG_LOGS = PATH_LOGS + "/debug-logs.log";
        public const string FILE_PARAMETERS = "settings.conf";
        public const char TOKEN_PARAMETERS_SEPARATOR = '=';
        public const string PARAMETER_LANG = "lang";
        public const string PARAMETER_LANG_ALT = "lang_alternative";

        public IView View { get; set; }
        public ISaver ProjectSaver { get; set; }
        public IDictionary<string, string> Parameters { get; set; }
        public ILogger DebugLogger { get; set; }
        public ILogger SavesLogger { get; set; }
        public ILogger StateLogger { get; set; }
        private static readonly Lazy<EasySaveController> _instance = new Lazy<EasySaveController>(() => new EasySaveController(), true);
        public static EasySaveController Instance { get => _instance.Value; }

        /// <summary>
        /// The parameter parser.
        /// </summary>
        private KeyValueParser ParamParser { get; }

        private EasySaveController() {
            ParamParser = new KeyValueParser(TOKEN_PARAMETERS_SEPARATOR);
        }

        /// <summary>
        /// Initialize localizer by loading the langs folder
        /// Initialize the current lang to english if found
        /// If not found, initialize it to the to the first
        /// language found.
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void LoadLocalizer() {
            DebugLogger.Debug(string.Format("Loading {0} as langs folder", PATH_LANGS));
            Localizer.Instance.LoadFolder(PATH_LANGS);
            try {
                Localizer.Instance.CurrentLang = Localizer.Instance.GetLang(Parameters[PARAMETER_LANG]);
                
            }
            catch(Exception) {
                if(Localizer.Instance.Langs.Count == 0) {
                    DebugLogger.Error("Could not load any lang file !");
                    throw new Exception("Could not load any lang file");
                }
                else {
                    Localizer.Instance.CurrentLang = Localizer.Instance.Langs[0];
                }
            }
            DebugLogger.Debug(string.Format("Loaded lang {0} as current lang", Localizer.Instance.CurrentLang.Tag));
            if (Parameters.ContainsKey(PARAMETER_LANG_ALT)) {
                try {
                    Localizer.Instance.DefaultLang = Localizer.Instance.GetLang(Parameters[PARAMETER_LANG_ALT]);
                    DebugLogger.Debug(string.Format("Loaded lang {0} as alternative lang", Localizer.Instance.DefaultLang.Tag));
                }
                catch (Exception) {
                    DebugLogger.Error("Could not load alternative lang file !");
                    throw new Exception(Localizer.Instance.Localize("lang.error.alternative"));
                }
            }
        }

        /// <summary>
        /// Register all available save loaders
        /// </summary>
        private void RegisterSaveLoaders() {
            SaveLoader.SaveLoadersRegistry["xml"] = new SaveLoaderXml();
        }

        /// <summary>
        /// Initialize and load saver
        /// </summary>
        /// <exception cref="Exception"></exception>
        private void LoadSaver() {
            ProjectSaver = new Saver();
            DebugLogger.Debug(string.Format("Loading {0} as save project folder", PATH_PROJECTS));
            ProjectSaver.LoadAllSaves(PATH_PROJECTS);
        }

        /// <summary>
        /// Initialize and load parameters
        /// </summary>
        private void LoadParameters() {
            KeyValueParser parser = new KeyValueParser(TOKEN_PARAMETERS_SEPARATOR);
            Parameters = parser.ParseFile(FILE_PARAMETERS);
        }

        /// <summary>
        /// Initialize and load all loggers
        /// </summary>
        private void LoadLogger() {
            DebugLogger = new Logger(FILE_DEBUG_LOGS);
            SavesLogger = new Logger(PATH_SAVES_LOGS+"/"+ DateTime.Now.ToString("dd-MM-yyyy")+".log");
            StateLogger = new Logger(PATH_STATE_LOGS);
        }

        public void Terminate() {
            ProjectSaver.WriteAllSaves(PATH_PROJECTS, Saver.DEFAULT_EXTENSION);
            ParamParser.ToFile(FILE_PARAMETERS, Parameters);

            DebugLogger.Info("Program terminated successfully");
        }

        public void Main(string[] args) {
            LoadLogger();
            DebugLogger.Debug("");
            DebugLogger.Debug(string.Format("Started with arguments: {0}", args.Length>0?args.Aggregate((l, r) => l + " " + r):""));
            LoadParameters();
            LoadLocalizer();
            RegisterSaveLoaders();
            LoadSaver();

            if(args.Length > 0) {
                EasySaveViews.EasySaveConsole.ParentController = this;
                View = EasySaveViews.EasySaveConsole.Instance;
            }
            else {
                EasySaveGUI.App.ParentController = this;
                View = EasySaveGUI.App.Instance;
            }
            
            View.DisplayedSaveProjects = ProjectSaver.Saves;
            View.Start(args);

            Terminate();
        }

        public void CreateSaveProject(ISave save) {
            DebugLogger.Info(string.Format("Created {0} save project", save.Name));
            ProjectSaver.Saves.Add(save);
        }

        public void EditSaveProject(string name, ISave newSave) {
            DebugLogger.Info(string.Format("Editing {0} save project", name));
            int index = ((List<ISave>)ProjectSaver.Saves).FindIndex(s => s.Name == name);
            if (index == -1) throw new NullReferenceException(); //Could not find save project

            Type t = typeof(ISave);
            var properties = t.GetProperties();
            foreach(var prop in properties) {
                var newVal = prop.GetValue(newSave);
                var oldVal = prop.GetValue(ProjectSaver.Saves[index]);

                if (oldVal == null && newVal != null) {
                    prop.SetValue(ProjectSaver.Saves[index], newVal);
                }
                else if(oldVal != null && newVal == null) {
                    prop.SetValue(newSave, oldVal);
                }
            }

            if (newSave.Name != null) {
                DeleteSaveProject(name);
                CreateSaveProject(newSave);
            }
        }

        public void DeleteSaveProject(string name) {
            DebugLogger.Info(string.Format("Deleting {0} save project", name));
            ProjectSaver.Saves.RemoveAt(((List<ISave>)ProjectSaver.Saves).FindIndex(s => s.Name == name));
            string file = Directory.GetFiles(PATH_PROJECTS).Foreach(f => Path.GetFileName(f)).First(f => f.Substring(0,f.LastIndexOf('.'))==name);
            File.Delete(PATH_PROJECTS+'/'+file);
        }

        public void ExecuteSaveProjects(object names) {
            if (Utils.IsBusinessSoftwareRunning()) return;
            IList<ISave> selectedSaves = ProjectSaver.Saves.GenericIntersect((IList<string>)names, (s, n) => s.Name == n).ToList();
            ProjectSaver.ExecuteSaves(selectedSaves, this);
        }

        public void SetParameter(string name, string value) {
            Parameters[name] = value;
        }
    }
}
