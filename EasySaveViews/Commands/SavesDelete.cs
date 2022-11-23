using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews.Commands {
    /// <inheritdoc cref="Command"/>
    /// Delete a save project
    class SavesDelete : Command {
        public override string Name => Localizer.Instance.Localize("command.saves.delete");
        public override string Description => Localizer.Instance.Localize("command.saves.delete.description");

        private string SaveName { get; set; }

        public SavesDelete() {
            Parameters.Add(new Parameter(PARAM_GENERIC_NAME, Localizer.Instance.Localize("command.saves.delete.name.description"), true, (_1, _2, v) => SaveName = v));
        }

        public override int Call(string[] args) {
            ICallArgs callArgs = ParseArgs(args[1..]);
            CallParametersCallbacks(callArgs);
            CheckMandatValue(SaveName, PARAM_GENERIC_NAME);
            EasySaveConsole.ParentController.DeleteSaveProject(SaveName);
            if (!IsQuiet(callArgs))
                Console.WriteLine(Localizer.Instance.Localize("command.saves.delete.success"));
            return 0;
        }
    }
}
