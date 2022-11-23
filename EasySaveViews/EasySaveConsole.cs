using EasySave;
using System;
using System.Collections.Generic;

namespace EasySaveViews
{
    /// <summary>
    /// A console interface which works with command
    /// line arguments
    /// </summary>
    public class EasySaveConsole : IView
    {
        /// <value>
        /// The returned status code when help is displayed
        /// </value>
        public const int RETURN_CODE_HELP = 1;

        /// <value>
        /// The returned status code when everything goes well
        /// </value>
        public const int RETURN_CODE_SUCCESS = 0;

        public IList<ISave> DisplayedSaveProjects { get; set; }
        public ExecuteSavesCallback ExecuteSaves { get; set; }
        public static IEasySaveController ParentController { get; set; }
        private static readonly Lazy<EasySaveConsole> _instance = new Lazy<EasySaveConsole>(() => new EasySaveConsole(), true);
        public static EasySaveConsole Instance { get => _instance.Value; }

        /// <summary>
        /// Display the main help with all possible sub commands
        /// and their possible parameters with no more informations
        /// </summary>
        private void DisplayHelp()
        {
            Console.WriteLine("==================== EasySave ====================");
            foreach (var cmd in Command.CommandRegistry)
            {
                Console.Write(cmd.Name + " ");
                foreach (var p in cmd.Parameters)
                {
                    Console.Write(Command.TOKEN_CHAR_PARAM + p.Name + " ");
                }
                Console.WriteLine();
            }
        }

        public void Error(string description)
        {
            string er = string.Format(Localizer.Instance.Localize("console.common.error"), description);
            Console.WriteLine(er);
            ParentController.DebugLogger.Error(er);
        }

        public void FatalError(string description)
        {
            Error(description);
            throw new Exception(Localizer.Instance.Localize("console.common.error.fatal"));
        }

        private EasySaveConsole()
        {
            Command.CommandRegistry.Add(new Commands.Saves());
            Command.CommandRegistry.Add(new Commands.Config());
        }

        public int Exit()
        {
            //Not used in console version but could be usefull for
            //gui where another thread need to be shutted down
            return 0;
        }

        public int Start(string[] args)
        {
            if (args.Length == 0 || args[0] == "")
            {
                //TODO: Centralized version
                //Console.WriteLine("EasySave v1.0");
                Console.WriteLine(Localizer.Instance.Localize("console.main.help"));
                //Console.WriteLine("Please run 'EasySave -h' or 'EasySave <command> -h' for help menu.");
                return RETURN_CODE_HELP;
            }
            else if (args[0] == (Command.TOKEN_CHAR_PARAM + "h"))
            {
                DisplayHelp();
                return RETURN_CODE_HELP;
            }

            try
            {
                return Command.Execute(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return RETURN_CODE_SUCCESS;
        }

    }
}
