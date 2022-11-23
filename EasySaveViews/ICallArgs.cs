using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews {
    /// <summary>
    /// Contains a parsed version of command line
    /// arguments and tools to handle it
    /// </summary>
    interface ICallArgs {
        /// <summary>
        /// Get a parameter value by its name
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The value of the parameter</returns>
        string this[string name] { get; }

        /// <value>
        /// List of parsed parameters with their values if
        /// there is one
        /// </value>
        IList<IMutablePair<IParameter, string>> Parameters { get; }

        /// <summary>
        /// Check if a parameter is included
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>true if the parameter is included otherwise false</returns>
        bool HasParameter(string name);

        /// <summary>
        /// Get an optional parameter's value by its name
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <returns>The value of the targeted parameter</returns>
        string GetOptParam(string name);

        /// <summary>
        /// Get a mandatory parameter's value by its name otherwise
        /// execute an action
        /// </summary>
        /// <param name="name">The name of the parameter</param>
        /// <param name="ifNotFound">The action to execute if not found</param>
        /// <returns>The value of the targeted parameter</returns>
        string GetMandatParam(string name, Action ifNotFound);
    }
}
