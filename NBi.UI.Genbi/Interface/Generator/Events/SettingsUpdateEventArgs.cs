using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.Generator.Events
{
    public class SettingsUpdateEventArgs : EventArgs
    {
        public string Name { get; private set; }
        public string Value { get; private set; }

        public SettingsUpdateEventArgs(string name, string value)
        {
            Name = name;
            Value = value;
        }

    }
}
