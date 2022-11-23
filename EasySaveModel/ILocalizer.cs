using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave {
    /// <summary>
    /// Represent a language collection and provide
    /// methods to hndle them
    /// </summary>
    public interface ILocalizer {
        /// <value>
        /// The raw language collection
        /// </value>
        IList<ILang> Langs { get; }

        /// <value>
        /// The current language to which the text
        /// will be translated by Localize.
        /// </value>
        /// <seealso cref="DefaultLang"/>
        /// <seealso cref="Localize(string)"/>
        ILang CurrentLang { get; set; }

        /// <value>
        /// The language to which the text
        /// will be translated if not translation
        /// is found in the <c>CurrentLang</c>
        /// </value>
        /// <seealso cref="CurrentLang"/>
        /// <seealso cref="Localize(string)"/>
        ILang DefaultLang { get; set; }

        /// <summary>
        /// Load all .lang files from a folder
        /// </summary>
        /// <param name="folder">The path to the folder to
        /// load from</param>
        void LoadFolder(string folder);

        /// <summary>
        /// Load a single .lang file
        /// </summary>
        /// <param name="file">The path to the file to
        /// load</param>
        void LoadLang(string file);

        /// <summary>
        /// Translate a code to a readable string
        /// </summary>
        /// <param name="code">The code of the string
        /// to use the translation</param>
        /// <returns>The translated string</returns>
        string Localize(string code);

        /// <summary>
        /// Get a specific language by its tag
        /// </summary>
        /// <param name="tag">The tag of the needed langage
        /// </param>
        /// <returns>The language</returns>
        ILang GetLang(string tag);

        /// <summary>
        /// Translate a code to a readable string and format it with
        /// <c>string.Format</c>
        /// </summary>
        /// <param name="code">The code of the string
        /// to use the translation</param>
        /// <param name="args">The format arguments</param>
        /// <returns>The translated string</returns>
        /// <seealso cref="string.Format(string, object?)"/>
        string FormatLocalize(string code, params object[] args);
    }
}
