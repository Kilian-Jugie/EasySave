using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EasySave
{
    public class Saver : ISaver {
        public const string DEFAULT_EXTENSION = "xml";
        public IList<ISave> Saves { get; }

        public Saver() {
            Saves = new List<ISave>();
        }

        public void ExecuteSaves(IList<ISave> ISaves, IEasySaveController controller) {
            foreach (ISave save in ISaves)  {
                string[] toLog = { string.Format("Executing save '{0}' on {1} with parameters :", save.Name, DateTime.Now), 
                    string.Format("\t-From: {0}", save.PathFrom), string.Format("\t-To: {0}", save.PathTo),
                    string.Format("\t-Type: {0}", save.Type) };
                controller.SavesLogger.Info(toLog);
                controller.DebugLogger.Info(toLog);
                SaveType.GetTypeFromName(save.Type).Process.Invoke(save,controller);
            }
        }

        public void LoadAllSaves(string folder) {
            string[] files = Directory.GetFiles(folder);
            foreach(var f in files) {
                try {
                    var loader = SaveLoader.SaveLoadersRegistry[f[(f.LastIndexOf('.')+1)..]];
                    Saves.Add(loader.Load(f));
                }catch(KeyNotFoundException) {
                    throw new Exception(Localizer.Instance.FormatLocalize("saver.loadall.unknown.extension", f[(f.LastIndexOf('.')+1)..]));
                }
            }
        }

        public void WriteAllSaves(string folder, string extension = DEFAULT_EXTENSION) {
            foreach (var s in Saves) {
                try {
                    var loader = SaveLoader.SaveLoadersRegistry[extension];
                    loader.Write(folder, s);
                }
                catch (KeyNotFoundException) {
                    throw new Exception(Localizer.Instance.FormatLocalize("saver.writeall.unknown.extension", extension));
                }
            }
        }
    }
}
