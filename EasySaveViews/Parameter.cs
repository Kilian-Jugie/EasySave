using System;
using System.Collections.Generic;
using System.Text;

namespace EasySaveViews {
    /// <inheritdoc cref="IParameter"/>
    class Parameter : IParameter {
        public string Name { get; }
        public string Description { get; }
        public bool TakeValue { get; }
        public ParameterCallback Callback { get; }

        public Parameter(string name, string description, bool takeValue, ParameterCallback callback=null) {
            Name = name;
            TakeValue = takeValue;
            Callback = callback;
            Description = description;
        }
    }
}
