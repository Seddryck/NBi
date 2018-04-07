using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.View.TestSuiteGenerator
{
    public partial class SettingsControl : UserControl
    {
        public SettingsControl()
        {
            InitializeComponent();
        }

        internal void DataBind(SettingsPresenter presenter)
        {
            if (presenter != null)
            {
                settingsName.DataSource = presenter.SettingsNames;

                settingsName.DataBindings.Add("SelectedItem", presenter, "SettingsNameSelected", true, DataSourceUpdateMode.OnValidation);
                settingsName.SelectedIndexChanged += (s, args) => settingsName.DataBindings["SelectedItem"].WriteValue();
                settingsValue.TextChanged += (s, args) => presenter.UpdateValue((string)settingsName.SelectedValue, settingsValue.Text);
                settingsValue.DataBindings.Add("Text", presenter, "SettingsValue", true, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void toto(object s, EventArgs args)
        {
            MessageBox.Show("Hello");
        }


        public Button AddCommand
        {
            get {return addReference;}
        }

        public Button RemoveCommand
        {
            get { return removeReference; }
        }


    }
}
