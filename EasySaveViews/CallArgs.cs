using EasySave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace EasySaveViews {
    /// <inheritdoc cref="ICallArgs"/>
    class CallArgs : ICallArgs {
        public IList<IMutablePair<IParameter, string>> Parameters { get; }

        public CallArgs() {
            Parameters = new List<IMutablePair<IParameter, string>>();
        }

        public bool HasParameter(string name) {
            return Parameters.FirstOrDefault(pair => pair.Item1.Name == name) != default;
        }

        public string GetOptParam(string name) {
            if (HasParameter(name)) return this[name];
            return null;
        }

        public string GetMandatParam(string name, Action ifNotFound) {
            if (!HasParameter(name)) {
                ifNotFound();
                return null;
            }
            return this[name];
        }

        public string this[string name] {
            get => Parameters.First(p => p.Item1.Name == name)?.Item2;
        }
    }
}
