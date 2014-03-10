using System;
using System.ComponentModel;
using System.Linq;

namespace NBi.UI.Genbi.Interface
{
    interface ISettingsView : IView
    {
        BindingList<string> SettingsNames { get; set; }
        string SettingsValue { get; set; }


        int SettingsSelectedIndex { get; set; }
    }
}
