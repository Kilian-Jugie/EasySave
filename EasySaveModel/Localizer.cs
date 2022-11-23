using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace EasySave {
    /// <summary>
    /// <see cref="ILocalizer"/>
    /// </summary>
    public class Localizer : ILocalizer {
        public IList<ILang> Langs { get; }
        public ILang CurrentLang { get; set; }
        public ILang DefaultLang { get; set; }
        private static readonly Lazy<Localizer> _instance = new Lazy<Localizer>(() => new Localizer(), true);
        public static Localizer Instance { get => _instance.Value; }

        private Localizer() {
            Langs = new List<ILang>();
        }

        /// <inheritdoc/>
        /// <exception cref="Exception"></exception>
        public string Localize(string code) {
            string translation;
            if (CurrentLang.Translations.ContainsKey(code))
                translation = CurrentLang.Translations[code];
            else if (DefaultLang != null && DefaultLang.Translations.ContainsKey(code))
                translation = DefaultLang.Translations[code];
            else {
                try {
                    var lang = Langs.First(l => l.Translations.ContainsKey(code));
                    translation = lang.Translations[code];
                }catch(Exception) {
                    throw new Exception(string.Format(Localize("localizer.unknown.code"), code));
                }
            }

            return translation;
        }

        public string FormatLocalize(string code, params object[] args) {
            return string.Format(Localize(code), args);
        }

        public void LoadFolder(string folder) {
            string[] files = Directory.GetFiles(folder, "*.lang");
            foreach(var f in files) {
                LoadLang(f);
            }
        }

        public void LoadLang(string file) {
            Langs.Add(Lang.LoadFromFile(file));
        }

        public ILang GetLang(string tag) {
            return Langs.First(l => l.Tag == tag);
        }
    }
}
