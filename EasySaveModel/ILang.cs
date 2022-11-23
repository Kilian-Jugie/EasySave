using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave {
    /// <summary>
    /// Define a language and contains all translations from code
    /// to this language.
    /// </summary>
    public interface ILang {
        /// <value>
        /// Contains translations indexed by code
        /// </value>
        IDictionary<string, string> Translations { get; }

        /// <value>
        /// The readable name of the language
        /// e.g: English
        /// </value>
        string Name { get; }

        /// <value>
        /// The IETF language tag which contains the language and
        /// a language extension
        /// </value>
        string Tag { get; }

        /// <summary>
        /// Get a translation by its code
        /// </summary>
        /// <param name="code">The code of the string to translate
        /// </param>
        /// <returns>The translation</returns>
        string this[string code] { get; set; }
    }
}
