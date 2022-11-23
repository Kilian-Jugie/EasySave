using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews.Commands {
    /// <inheritdoc cref="Command"/>
    /// <summary>
    /// Manage program settings
    /// </summary>
    class Config : Command {
        public override string Name => Localizer.Instance.Localize("command.config");
        public override string Description => Localizer.Instance.Localize("command.config.description");

        public Config() {
            SubCommands.Add(new ConfigList());
            SubCommands.Add(new ConfigSet());
        }

        public override int Call(string[] args) {
            ICallArgs callArgs = ParseArgs(args);
            if (callArgs == null) {
                return ExecuteSubCommand(args);
            }
            CallParametersCallbacks(callArgs);
            return 0;
        }
    }
}
