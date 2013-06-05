using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface.TestSuiteGenerator.Events;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class SettingsControl : UserControl
    {
        internal TestSuiteViewAdapter Adapter { get; set; }

        public SettingsControl()
        {
            InitializeComponent();
        }

        public string Value
        {
            get { return settingsValue.Text; }
            set { settingsValue.Text = value; }
        }


        public BindingList<string> Names
        {
            get
            {
                return (BindingList<string>)(bindingSettings.DataSource);
            }
            set
            {
                bindingSettings.DataSource = value;
            }
        }
        
        #region UI Events

        private void SettingsName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (settingsName.SelectedValue != null)
            {
                settingsValue.TextChanged -= SettingsValue_TextChanged;
                Adapter.InvokeSettingsSelect(new SettingsSelectEventArgs((string)settingsName.SelectedValue));
                settingsValue.TextChanged += SettingsValue_TextChanged;
            }
        }

        private void SettingsValue_TextChanged(object sender, EventArgs e)
        {
            Adapter.InvokeSettingsUpdate(new SettingsUpdateEventArgs((string)settingsName.SelectedValue, Value));
        }

        #endregion

        internal void DeclareBindings()
        {
            settingsName.DataSource = bindingSettings;
        }


    }
}
