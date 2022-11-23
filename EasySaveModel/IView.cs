using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave
{
    /// <summary>
    /// <see cref="IView.ExecuteSaves"/>
    /// </summary>
    /// <param name="indexes">The array indexes from <c>DisplayedSaveProjects</c>
    /// of projects to execute</param>
    public delegate void ExecuteSavesCallback(IList<int> indexes);

    /// <summary>
    /// Represent the interface with the user : collect data from
    /// him and displaying results
    /// </summary>
    public interface IView
    {

        /// <summary>
        /// Collection of save projects which are displayed
        /// to the user
        /// </summary>
        /// <seealso cref="ISave"/>
        IList<ISave> DisplayedSaveProjects { get; set; }

        /// <summary>
        /// Function to call when user ask for save projects 
        /// execution
        /// </summary>
        ExecuteSavesCallback ExecuteSaves { get; set; }

        /// <summary>
        /// Start the interface
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>A status code where 0 is success</returns>
        int Start(string[] args);

        /// <summary>
        /// Stop the interface
        /// </summary>
        /// <returns>A status code where 0 is success</returns>
        int Exit();

        /// <summary>
        /// Display an error and log it
        /// </summary>
        /// <param name="description">The error</param>
        void Error(string description);

        /// <summary>
        /// Display an error, log it and stop execution
        /// </summary>
        /// <param name="description">The error</param>
        void FatalError(string description);
    }
}
