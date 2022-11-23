using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews.Commands {
    /// <inheritdoc cref="Command"/>
    /// Manage saves projects
    class Saves : Command {
        public override string Name => "saves";
        public override string Description => Localizer.Instance.Localize("command.saves.description");
        public const string PARAM_EDIT = "edit";
        public const string PARAM_LIST = "list";
        public const string PARAM_SHOW = "show";
        public const string PARAM_CREATE = "create";
        public const string PARAM_DELETE = "delete";
        public const string PARAM_EXECUTE = "execute";

        public Saves() {
            SubCommands.Add(new SavesList());
            SubCommands.Add(new SavesCreate());
            SubCommands.Add(new SavesDelete());
            SubCommands.Add(new SavesEdit());
            SubCommands.Add(new SavesExecute());
        }

        public override int Call(string[] args) {
            ICallArgs callArgs = ParseArgs(args);
            if(callArgs == null) {
                return ExecuteSubCommand(args);
            }
            CallParametersCallbacks(callArgs);
            return 0;
        }
    }
}
