using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews.Commands {
    /// <inheritdoc cref="Command"/>
    /// Create a new save project
    class SavesCreate : Command {
        public override string Name => Localizer.Instance.Localize("command.saves.create");
        public override string Description => Localizer.Instance.Localize("command.saves.create.description");

        private string SaveName { get; set; }
        private string SaveFrom { get; set; }
        private string SaveTo { get; set; }
        private string SaveType { get; set; }

        public SavesCreate() {
            Parameters.Add(new Parameter(PARAM_GENERIC_NAME, Localizer.Instance.Localize("command.saves.create.name.description"), true, (_1, _2, v) => SaveName = v));
            Parameters.Add(new Parameter(PARAM_GENERIC_FROM, Localizer.Instance.Localize("command.saves.create.from.description"), true, (_1, _2, v) => SaveFrom = v));
            Parameters.Add(new Parameter(PARAM_GENERIC_TO, Localizer.Instance.Localize("command.saves.create.to.description"), true, (_1, _2, v) => SaveTo = v));
            Parameters.Add(new Parameter(PARAM_GENERIC_TYPE, Localizer.Instance.Localize("command.saves.create.type.description"), true, (_1, _2, v) => SaveType = v));
        }

        public override int Call(string[] args) {
            ICallArgs callArgs = ParseArgs(args[1..]);
            CallParametersCallbacks(callArgs);
            CheckMandatValue(SaveName, PARAM_GENERIC_NAME);
            ISave save = new Save() { Name = SaveName, PathFrom = SaveFrom, PathTo = SaveTo, Type = SaveType };
            EasySaveConsole.ParentController.CreateSaveProject(save);
            if (!IsQuiet(callArgs))
                Console.WriteLine(Localizer.Instance.Localize("command.saves.create.success"));
            return 0;
        }
    }
}
