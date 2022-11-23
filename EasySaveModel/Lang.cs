using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace EasySave {
    /// <summary>
    /// <see cref="ILang"/>
    /// </summary>
    public class Lang : ILang {
        /// <value>
        /// The static field of .lang file which
        /// contains the translated name of the
        /// language.
        /// </value>
        const string LANG_NAME_CODE = "local.name";

        public const char TOKEN_LANG_SEPARATOR = '=';

        public IDictionary<string, string> Translations { get; }
        public string Name { get; }
        public string Tag { get; }

        public string this[string code] { get => Translations[code]; set => Translations[code] = value; }

        public Lang(string name, string tag, IDictionary<string, string> translations) {
            Translations = translations;
            Name = name;
            Tag = tag;
        }

        /// <summary>
        /// Load a lang from a file
        /// </summary>
        /// <param name="file">The path to the file to load
        /// the language from</param>
        /// <returns>The new created language</returns>
        public static ILang LoadFromFile(string file) {
            string name, tag;
            IDictionary<string, string> translations;
            tag = Path.GetFileName(file);
            tag = tag.Substring(0, tag.LastIndexOf('.'));
            /*string fileContent = File.ReadAllText(file);
            string[] lines = fileContent.Trim().Replace("\r", "").Split('\n');
            foreach(var line in lines) {
                if (line.Length == 0) continue;
                int equalIndex = line.IndexOf('=');
                translations.Add(line.Substring(0, equalIndex), line.Substring(equalIndex + 1));
            }*/
            KeyValueParser parser = new KeyValueParser(TOKEN_LANG_SEPARATOR);
            translations = parser.ParseFile(file);
            name = translations[LANG_NAME_CODE];
            return new Lang(name, tag, translations);
        }

        public override string ToString() {
            return Name;
        }
    }
}
