using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews.Commands {
    /// <inheritdoc cref="Command"/>
    /// Edit a save project
    class SavesEdit : Command {
        public override string Name => Localizer.Instance.Localize("command.saves.edit");
        public override string Description => Localizer.Instance.Localize("command.saves.edit.description");
        public const string PARAM_EDIT_NAME = "newname";

        private string SaveName { get; set; }
        private string SaveNewName { get; set; }
        private string SaveFrom { get; set; }
        private string SaveTo { get; set; }
        private string SaveType { get; set; }

        public SavesEdit() {
            Parameters.Add(new Parameter(PARAM_GENERIC_NAME, Localizer.Instance.Localize("command.saves.edit.name.description"), true, (_1, _2, v) => SaveName = v));
            Parameters.Add(new Parameter(PARAM_EDIT_NAME, Localizer.Instance.Localize("command.saves.edit.newname.description"), true, (_1, _2, v) => SaveNewName = v));
            Parameters.Add(new Parameter(PARAM_GENERIC_FROM, Localizer.Instance.Localize("command.saves.edit.from.description"), true, (_1, _2, v) => SaveFrom = v));
            Parameters.Add(new Parameter(PARAM_GENERIC_TO, Localizer.Instance.Localize("command.saves.edit.to.description"), true, (_1, _2, v) => SaveTo = v));
            Parameters.Add(new Parameter(PARAM_GENERIC_TYPE, Localizer.Instance.Localize("command.saves.edit.type.description"), true, (_1, _2, v) => SaveType = v));
        }


        public override int Call(string[] args) {
            ICallArgs callArgs = ParseArgs(args[1..]);
            CallParametersCallbacks(callArgs);
            CheckMandatValue(SaveName, PARAM_GENERIC_NAME);
            ISave save = new Save() { Name = SaveNewName, PathFrom = SaveFrom, PathTo = SaveTo, Type = SaveType };
            EasySaveConsole.ParentController.EditSaveProject(SaveName, save);
            if (!IsQuiet(callArgs))
                Console.WriteLine(Localizer.Instance.Localize("command.saves.edit.success"));
            return 0;
        }
    }
}
