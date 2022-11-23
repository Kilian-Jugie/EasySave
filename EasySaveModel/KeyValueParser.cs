using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace EasySave {
    /// <summary>
    /// KeyValue pair parser
    /// Used to parse texts like:
    /// key1=value1
    /// key2=value2
    /// </summary>
    public class KeyValueParser {
        /// <value>
        /// The separator character which mark the
        /// key&value separation
        /// </value>
        private readonly char _separator;

        public KeyValueParser(char separator) {
            _separator = separator;
        }

        /// <summary>
        /// Parse an entire file with <c>Parse</c>
        /// </summary>
        /// <param name="filename">The filename of the file</param>
        /// <returns>A filled dictionnary with all keys & values of the file</returns>
        /// <seealso cref="Parse(string)"/>
        public IDictionary<string, string> ParseFile(string filename) {
            string fileContent = File.ReadAllText(filename);
            return Parse(fileContent);
        }

        /// <summary>
        /// Parse a text
        /// </summary>
        /// <param name="text">The text To parse</param>
        /// <returns>A filled dictionnary with all keys & values of the text</returns>
        /// <seealso cref="ParseFile(string)"/>
        public IDictionary<string, string> Parse(string text) {
            string[] lines = text.Replace("\r", "").Split('\n').Foreach(l => l.Trim()).ToArray();
            Dictionary<string, string> ret = new Dictionary<string, string>();
            foreach (var line in lines) {
                if (line.Length == 0) continue;
                int equalIndex = line.IndexOf(_separator);
                ret.Add(line[0..equalIndex], line[(equalIndex + 1)..]);
            }
            return ret;
        }

        /// <summary>
        /// Convert a key-value pair array to text
        /// </summary>
        /// <param name="keyValuePairs">The dictionnary to convert</param>
        /// <returns>A parse-ready text of key-values pairs separated by <c>_separator</c></returns>
        /// <seealso cref="ToFile(string, IDictionary{string, string})"/>
        public string ToString(IDictionary<string, string> keyValuePairs) {
            string ret = "";
            foreach(var it in keyValuePairs) {
                ret += it.Key + "=" + it.Value+"\n";
            }
            return ret;
        }

        /// <summary>
        /// Convert a key-value pair array to file
        /// </summary>
        /// <param name="keyValuePairs">The dictionnary to convert</param>
        /// <param name="filename">The filename where to write converted text</param>
        /// <seealso cref="ToString(IDictionary{string, string})"/>
        public void ToFile(string filename, IDictionary<string, string> keyValuePairs) {
            string conte = ToString(keyValuePairs);
            File.WriteAllText(filename, conte);
        }
    }
}
