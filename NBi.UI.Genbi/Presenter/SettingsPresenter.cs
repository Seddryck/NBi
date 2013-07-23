using System;
using System.ComponentModel;
using System.Linq;
using NBi.Service;
using NBi.Service.Dto;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.Settings;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Presenter
{
    class SettingsPresenter : BasePresenter<ISettingsView>
    {
        private readonly SettingsManager settingsManager;
        private string previousSelection;

        public SettingsPresenter(ISettingsView view, SettingsManager settingsManager)
            : base(view)
        {
            AddReferenceCommand = new AddReferenceCommand(this, new NewReferenceWindow());
            RemoveReferenceCommand = new RemoveReferenceCommand(this);

            this.settingsManager = settingsManager;
            Settings = new BindingList<Setting>();
            SettingsNames = new BindingList<string>();
            ReloadSettings();
            previousSelection = SettingsNames[0];

        }

        #region Bindable properties

        public string SettingsValue
        {
            get { return GetValue<string>("SettingsValue"); }
            set { SetValue("SettingsValue", value); }
        }

        public string SettingsNameSelected
        {
            get { return GetValue<string>("SettingsNameSelected"); }
            set 
            {
                SetValue("SettingsNameSelected", value);
                previousSelection = SettingsNameSelected;
            }
        }

        public BindingList<string> SettingsNames
        {
            get { return GetValue<BindingList<string>>("SettingsNames"); }
            private set { SetValue("SettingsNames", value); }
        }

        public BindingList<Setting> Settings
        {
            get { return GetValue<BindingList<Setting>>("Settings"); }
            private set { SetValue("Settings", value); }
        }

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            //TODO When properties are changed, call commands refresh
            switch (propertyName)
            {
                case "SettingsNameSelected":
                    UpdateValue(previousSelection, SettingsValue);
                    DisplayValue(SettingsNameSelected);
                    RemoveReferenceCommand.Refresh();
                    break;
                default:
                    break;
            }
        }

        public void DisplayValue(string name)
        {
            SettingsValue = settingsManager.GetValue(name);
        }

        public void UpdateValue(string name, string value)
        {
            //Ensure that the reference exists before updating this value
            //Needed because SetValue is creating the reference if it doesn't exist and when removing  reference it's a problem
            if (settingsManager.Exists(name))
                settingsManager.SetValue(name, value);
        }

        public void AddReference(string name)
        {
            settingsManager.Add(name, string.Empty);
            ReloadSettings();
            SettingsNameSelected = "Reference - " + name;
        }

        private void ReloadSettings()
        {
            SettingsNames.Clear();
            foreach (var name in settingsManager.GetNames())
                SettingsNames.Add(name);

            Settings.Clear();
            foreach (var setting in settingsManager.GetSettings())
                Settings.Add(setting);
        }

        public void RemoveReference(string name)
        {
            settingsManager.Remove(name);
            ReloadSettings();
        }

        public bool IsValidReferenceName(string name)
        {
            return settingsManager.IsValidReferenceName(name);
        }

        internal bool IsReferenceSelected()
        {
            return settingsManager.IsReferenceSelected(SettingsNameSelected);
        }

        public ICommand AddReferenceCommand { get; private set; }
        public ICommand RemoveReferenceCommand { get; private set; }

        public event EventHandler<EventArgs> ListUpdated;  
 
        public void OnListUpdated(EventArgs e)
        {
            EventHandler<EventArgs> handler = ListUpdated;
            if (handler != null)
                handler(this, e);
        }
    }
}
