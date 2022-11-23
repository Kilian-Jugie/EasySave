using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews.Commands {
    /// <inheritdoc cref="Command"/>
    /// List all available save projects
    class SavesList : Command {
        public override string Name => Localizer.Instance.Localize("command.saves.list");
        public override string Description => Localizer.Instance.Localize("command.saves.list.description");

        public override int Call(string[] args) {
            ICallArgs callArgs = ParseArgs(args[1..]);
            CallParametersCallbacks(callArgs);
            foreach (var p in EasySaveConsole.Instance.DisplayedSaveProjects) {
                Console.WriteLine(string.Format("{0}: [{1}] {2} -> {3}", p.Name, p.Type, p.PathFrom, p.PathTo));
            }
            return 0;
        }
    }
}
