using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave {
    /// <summary>
    /// Controller class which initialize
    /// ressources and link model with
    /// view
    /// </summary>
    public interface IEasySaveController {

        public const string PATH_PROJECTS = "../projects";
        /// <value>
        /// <see cref="IView"/>
        /// </value>
        IView View { get; set; }

        /// <value>
        /// <see cref="ISaver"/>
        /// </value>
        ISaver ProjectSaver { get; set; }

        /// <value>
        /// Associative list of all parameters and their value
        /// </value>
        IDictionary<string, string> Parameters { get; }

        /// <value>
        /// A complete log where all informations should be described
        /// </value>
        ILogger DebugLogger { get; set; }

        /// <value>
        /// A daily log where all saves' executions are described
        /// </value>
        ILogger SavesLogger { get; set; }

        /// <summary>
        /// A log where real-time data about the current executing save are logged
        /// </summary>
        ILogger StateLogger { get; set; }

        /// <summary>
        /// Create a new save project
        /// </summary>
        /// <param name="save">The save project to create</param>
        void CreateSaveProject(ISave save);

        /// <summary>
        /// Edit an existing save project
        /// </summary>
        /// <param name="name">The name of the project to edit</param>
        /// <param name="newSave">The new parameters. Parameters null to let old ones</param>
        void EditSaveProject(string name, ISave newSave);

        /// <summary>
        /// Delete an existing save project
        /// </summary>
        /// <param name="name">The name of the project to delete</param>
        void DeleteSaveProject(string name);

        /// <summary>
        /// Execute one ore more save projects
        /// </summary>
        /// <param name="names">Names of projects to execute</param>
        void ExecuteSaveProjects(object names);

        /// <summary>
        /// Set the value of one parameter
        /// </summary>
        /// <param name="name">The name of the parameter to set</param>
        /// <param name="value">The new value</param>
        void SetParameter(string name, string value);

        /// <summary>
        /// The controller entry point. Must be called
        /// by program entry point with the command line
        /// arguments
        /// </summary>
        /// <param name="args">The cmdline arguments</param>
        void Main(string[] args);
        void Terminate();
    }
}
