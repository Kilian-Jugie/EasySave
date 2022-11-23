using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using EasySave;

namespace EasySaveViews {
    /// <inheritdoc cref="ICommand"/>
    abstract class Command : ICommand {
        /// <value>
        /// The character which indicate a parameter
        /// name
        /// </value>
        public const char TOKEN_CHAR_PARAM = '-';
        public const string PARAM_GENERIC_Q = "q";
        public const string PARAM_GENERIC_QUIET = "quiet";
        public const string PARAM_GENERIC_FROM = "from";
        public const string PARAM_GENERIC_TO = "to";
        public const string PARAM_GENERIC_TYPE = "type";
        public const string PARAM_GENERIC_NAME = "name";
        public const string PARAM_GENERIC_VALUE = "value";

        public abstract string Name { get; }
        public abstract string Description { get; }
        public IList<IParameter> Parameters { get; }
        public IList<ICommand> SubCommands { get; }
        

        /// <value>
        /// List of all available commands
        /// </value>
        public static List<ICommand> CommandRegistry;

        static Command() {
            CommandRegistry = new List<ICommand>();
        } 

        /// <summary>
        /// Command constructor initializing view and creating 2
        /// default help parameter : h and help
        /// </summary>
        /// <param name="view">The command view</param>
        public Command() {
            Parameters = new List<IParameter>();
            SubCommands = new List<ICommand>();
            Parameters.Add(new Parameter("h", Localizer.Instance.Localize("console.common.help.desc"), false, DisplayHelp));
            Parameters.Add(new Parameter("help", Parameters.Last().Description, false, DisplayHelp));
            Parameters.Add(new Parameter("q", Localizer.Instance.Localize("console.common.quiet.desc"), false, null));
            Parameters.Add(new Parameter("quiet", Parameters.Last().Description, false, null));
        }


        //TODO: COMMAND & SUBCOMMAND UNIFORMIZATION

        /// <summary>
        /// Execute the command line arguments by finding the called
        /// command and calling its <c>ICommand.Call</c> method
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <param name="view">View to execute command into</param>
        /// <returns>A status code where 0 is success</returns>
        /// <exception cref="Exception"></exception>
        /// <seealso cref="ICommand.Call(string[])"/>
        public static int Execute(string[] args) {
            try {
                return CommandRegistry.Find(cmd => cmd.Name == args[0]).Call(args[1..]);
            }
            catch(NullReferenceException) {
                throw new Exception(Localizer.Instance.Localize("console.common.unknown.command"));
            }
        }

        /// <summary>
        /// Execute the command line sub-commands by finding the called
        /// command and calling its <c>ICommand.Call</c> method
        /// </summary>
        /// <param name="args">Command line arguments</param>
        /// <returns>A status code where 0 is success</returns>
        /// <exception cref="Exception"></exception>
        /// <seealso cref="ICommand.Call(string[])"/>
        public int ExecuteSubCommand(string[] args) {
            try {
                return SubCommands.First(cmd => cmd.Name == args[0]).Call(args);
            }
            catch (InvalidOperationException) {
                throw new Exception(Localizer.Instance.Localize("console.common.unknown.subcommand"));
            }
        }

        /// <inheritdoc cref="ICommand.Call(string[])"/>
        public abstract int Call(string[] args);

        /// <summary>
        /// Parse command line arguments into <c>ICallArgs</c> format
        /// to make it easier to handle 
        /// </summary>
        /// <param name="args">The command line arguments</param>
        /// <returns>The new created ICallArgs</returns>
        /// <exception cref="Exception"></exception>
        protected ICallArgs ParseArgs(string[] args) {
            ICallArgs ret = new CallArgs();
            for (int i = 0; i < args.Length; ++i) {
                if (args[i][0] == TOKEN_CHAR_PARAM) {
                    IParameter param = Parameters.FirstOrDefault(p => p.Name == args[i][1..]);
                    if(param == default) {
                        throw new Exception(string.Format(Localizer.Instance.Localize("console.common.unknown.parameter"), args[i], Name));
                    }
                    ret.Parameters.Add(new MutablePair<IParameter, string>(param, null));
                }
                else {
                    if(ret.Parameters.Count == 0) {
                        return null; //This mean that we are not the last subcommand called
                    }
                    if(!ret.Parameters.Last().Item1.TakeValue) {
                        throw new Exception(string.Format(Localizer.Instance.Localize("console.common.mismatch.value"), ret.Parameters.Last().Item1.Name, args[i]));
                    }
                    ret.Parameters.Last().Item2 = args[i];
                }
            }
            return ret;
        }

        /// <summary>
        /// Check if a value is not null otherwise throw an exception
        /// with details
        /// </summary>
        /// <param name="value">The value to check</param>
        /// <param name="parameter">The parameter name from where the value is extracted</param>
        protected void CheckMandatValue(object value, string parameter) {
            if (value == null) throw new Exception(Localizer.Instance.FormatLocalize("console.common.required.parameter", parameter, Name));
        }
        
        /// <summary>
        /// Check if a quiet parameter is passed
        /// </summary>
        /// <param name="args">Parsed cmdline args</param>
        /// <returns>True if quiet is asked otherwise false</returns>
        protected bool IsQuiet(ICallArgs args) {
            return args.HasParameter(PARAM_GENERIC_Q) || args.HasParameter(PARAM_GENERIC_QUIET);
        }

        /// <summary>
        /// Call all callbacks of detected parameters
        /// </summary>
        /// <param name="args">The parsed command line arguments</param>
        protected void CallParametersCallbacks(ICallArgs args) {
            foreach(var p in Parameters) {
                if(args.HasParameter(p.Name)) {
                    if (p.Callback != null) p.Callback(this, args, args[p.Name]);
                }
            }
        }

        /// <summary>
        /// Display standard help with all possible parameters and
        /// their description
        /// </summary>
        /// <param name="cmd">The command from which we need to display help</param>
        /// <param name="args">Parsed command line arguments</param>
        public static void DisplayHelp(ICommand cmd, ICallArgs args, string v) {
            Console.Write(Localizer.Instance.Localize("global.name") + " " + cmd.Name);
            if(cmd.SubCommands.Count > 0) Console.Write(" " + Localizer.Instance.Localize("console.common.help.subcommand"));
            foreach (var p in cmd.Parameters) {
                Console.Write(" -" + p.Name);
                if(p.TakeValue) {
                    Console.Write(" " + Localizer.Instance.Localize("console.common.help.value"));
                }
            }
            Console.WriteLine();
            Console.WriteLine(Localizer.Instance.Localize("console.command.help.parameters")+":");
            foreach (var p in cmd.Parameters) {
                Console.Write("\t-" + p.Name);
                if (p.TakeValue) {
                    Console.Write(" " + Localizer.Instance.Localize("console.common.help.value"));
                }
                Console.WriteLine("\t" + p.Description);
            }
            Console.WriteLine();
            Console.WriteLine(Localizer.Instance.Localize("console.command.help.subcommands") + ":");
            foreach (var p in cmd.SubCommands) {
                Console.Write("\t" + p.Name);
                Console.WriteLine("\t" + p.Description);
            }
            Console.WriteLine();
        }
    }
}
