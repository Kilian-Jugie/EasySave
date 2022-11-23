using EasySave;
using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews.Commands {
    /// <inheritdoc cref="Command"/>
    /// <summary>
    /// Set a setting to a value
    /// </summary>
    class ConfigSet : Command {
        public override string Name => Localizer.Instance.Localize("command.config.set");
        public override string Description => Localizer.Instance.Localize("command.config.set.description");

        private string SettingValue { get; set; }
        private string SettingName { get; set; }

        public ConfigSet() {
            Parameters.Add(new Parameter(PARAM_GENERIC_NAME, Localizer.Instance.Localize("command.config.set.name.description"), true, (_1, _2, v) => SettingName = v));
            Parameters.Add(new Parameter(PARAM_GENERIC_VALUE, Localizer.Instance.Localize("command.config.set.value.description"), true, (_1, _2, v) => SettingValue = v));
        }

        public override int Call(string[] args) {
            ICallArgs callArgs = ParseArgs(args[1..]);
            CallParametersCallbacks(callArgs);
            CheckMandatValue(SettingName, PARAM_GENERIC_NAME);
            EasySaveConsole.ParentController.SetParameter(SettingName, SettingValue);
            if (!IsQuiet(callArgs))
                Console.WriteLine(Localizer.Instance.Localize("command.config.set.success"));
            return 0;
        }
    }
}
