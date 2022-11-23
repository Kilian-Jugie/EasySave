using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews {
    /// <summary>
    /// <see cref="IParameter.Callback"/>
    /// </summary>
    /// <param name="cmd">The parameter's parent command</param>
    /// <param name="args">The others arguments and values</param>
    /// <param name="val">The passed value otherwise null</param>
    delegate void ParameterCallback(ICommand cmd, ICallArgs args, string val);

    /// <summary>
    /// A sub command parameter which can or not take a value
    /// </summary>
    interface IParameter {
        /// <value>
        /// The name of the parameter. Also correspond to the
        /// text to enter to use this parameter
        /// </value>
        string Name { get; }

        /// <value>
        /// Describe if the parameter needs a value
        /// </value>
        bool TakeValue { get; }

        /// <value>
        /// The function to call when the parameter is encontered
        /// This is optional : if null, no functions will be called
        /// when encontered and parameter detection must be done
        /// manually.
        /// </value>
        /// <seealso cref="ParameterCallback"/>
        ParameterCallback Callback { get; }

        /// <value>
        /// The description of the parameter to print when command
        /// help is called
        /// </value>
        string Description { get; }
    }
}
