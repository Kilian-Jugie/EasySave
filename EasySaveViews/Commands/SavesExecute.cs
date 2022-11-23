using EasySave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EasySaveViews.Commands {
    /// <inheritdoc cref="Command"/>
    /// Execute one or more saves
    class SavesExecute : Command {
        public override string Name => Localizer.Instance.Localize("command.saves.execute");
        public override string Description => Localizer.Instance.Localize("command.saves.execute.description");

        private string SaveSelected { get; set; }

        public SavesExecute() {
            Parameters.Add(new Parameter(PARAM_GENERIC_NAME, Localizer.Instance.Localize("command.saves.execute.name.description"), true, (_1, _2, v) => SaveSelected = v));
        }

        public override int Call(string[] args) {
            ICallArgs callArgs = ParseArgs(args[1..]);
            CallParametersCallbacks(callArgs);
            CheckMandatValue(SaveSelected, PARAM_GENERIC_NAME);
            EasySaveConsole.ParentController.ExecuteSaveProjects(SaveSelected.Split(',').Foreach(s => s.Trim()).ToList());
            if (!IsQuiet(callArgs))
                Console.WriteLine(Localizer.Instance.Localize("command.saves.execute.success"));
            return 0;
        }

    }
}
