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
    class SettingsPresenter : PresenterBase
    {
        private readonly SettingsManager settingsManager;
        private string previousSelection;

        public SettingsPresenter(SettingsManager settingsManager, BindingList<Setting> settings)
            : base()
        {
            AddReferenceCommand = new AddReferenceCommand(this, new NewReferenceWindow());
            RemoveReferenceCommand = new RemoveReferenceCommand(this);

            this.settingsManager = settingsManager;
            Settings = settings;
            Refresh();
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
            var settingDisplayed = Settings.First(s => s.Name == name);
            if (settingDisplayed != null)
                SettingsValue = settingDisplayed.Value;
        }

        public void UpdateValue(string name, string value)
        {
            //Ensure that the reference exists before updating this value
            //Needed because SetValue is creating the reference if it doesn't exist and when removing  reference it's a problem
            var settingUpdated = Settings.FirstOrDefault(s => s.Name == name);
            if (settingUpdated != null)
                settingUpdated.Value = value;
            OnPropertyChanged("Settings");
        }

        public void AddReference(string name)
        {
            Settings.Add(new Setting() { Name = "Reference - " + name, Value = string.Empty });
            SettingsNames.Add("Reference - " + name);
            SettingsNameSelected = "Reference - " + name;
        }

        public void RemoveReference(string name)
        {
            var settingDeleted = Settings.FirstOrDefault(s => s.Name == name);
            if (settingDeleted != null)
            {
                Settings.Remove(settingDeleted);
                SettingsNames.Remove(name);
            }
            SettingsNameSelected = SettingsNames[0];

            OnPropertyChanged("Settings");
        }

        public bool IsValidReferenceName(string name)
        {
            return settingsManager.IsValidReferenceName(name);
        }

        internal bool IsReferenceSelected()
        {
            return !string.IsNullOrEmpty(SettingsNameSelected) && SettingsNameSelected.StartsWith("Reference - ");
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


        internal void Refresh()
        {
            SettingsNames = new BindingList<string>((Settings.Select<Setting, string>(s => s.Name)).ToList());
            previousSelection = SettingsNames[0];
            SettingsNameSelected = SettingsNames[0];
            DisplayValue(previousSelection);
        }
    }
}
