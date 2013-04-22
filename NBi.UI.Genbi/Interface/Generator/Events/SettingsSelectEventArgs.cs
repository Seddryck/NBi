using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Interface.Generator.Events
{
    public class SettingsSelectEventArgs : EventArgs
    {
        public string Name { get; private set; }
        public SettingsSelectEventArgs(string settingsName)
        {
            Name = settingsName;
        }

    }
}
