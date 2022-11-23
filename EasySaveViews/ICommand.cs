using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews {
    /// <summary>
    /// A subcommand to easysave
    /// e.g : easysave saves : saves is a ICommand
    /// </summary>
    interface ICommand {
        /// <value>
        /// Name of the command. Also the string to enter
        /// to call this command
        /// </summary>
        string Name { get; }

        /// <value>
        /// Description to write when help is called
        /// </value>
        string Description { get; }

        /// <value>
        /// List of possible parameters
        /// </value>
        IList<IParameter> Parameters { get; }

        /// <value>
        /// NOT IMPLEMENTED !
        /// List of possible subcommands
        /// </value>
        IList<ICommand> SubCommands { get; }

        /// <summary>
        /// Method to call when command is detected
        /// </summary>
        /// <param name="args">Command line args</param>
        /// <returns>A status where 0 is success</returns>
        int Call(string[] args);
    }
}
