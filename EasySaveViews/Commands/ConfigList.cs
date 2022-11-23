using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews.Commands {
    /// <inheritdoc cref="Command"/>
    /// <summary>
    /// List all available settings
    /// </summary>
    class ConfigList : Command {
        public override string Name => Localizer.Instance.Localize("command.config.list");
        public override string Description => Localizer.Instance.Localize("command.config.list.description");

        public override int Call(string[] args) {
            ICallArgs callArgs = ParseArgs(args[1..]);
            CallParametersCallbacks(callArgs);
            foreach (var p in EasySaveConsole.ParentController.Parameters) {
                Console.WriteLine(string.Format("{0}: {1}", p.Key, p.Value));
            }
            return 0;
        }
    }
}
