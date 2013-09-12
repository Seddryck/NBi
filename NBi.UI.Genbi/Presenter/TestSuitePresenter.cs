using System;
using System.ComponentModel;
using System.Linq;
using NBi.Service;
using NBi.Service.Dto;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.TestSuite;
using NBi.UI.Genbi.Interface;

namespace NBi.UI.Genbi.Presenter
{
    class TestSuitePresenter : BasePresenter<ITestSuiteView>
    {
        private readonly TestSuiteManager testSuiteManager;

        public TestSuitePresenter(ITestSuiteView testSuiteView, TestSuiteManager testSuiteManager)
            : base(testSuiteView)
        {
            this.testSuiteManager = testSuiteManager;
            this.SaveAsTestSuiteCommand = new SaveAsTestSuiteCommand(this);
            this.Tests = new BindingList<Test>();
            this.Settings = new BindingList<Setting>();
        }

        public ICommand SaveAsTestSuiteCommand { get; private set; }

        #region Bindable properties

        public BindingList<Test> Tests
        {
            get { return GetValue<BindingList<Test>>("Tests"); }
            set { SetValue("Tests", value); }
        }

        public BindingList<Setting> Settings
        {
            get { return GetValue<BindingList<Setting>>("Settings"); }
            set { SetValue("Settings", value); }
        }

        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            switch (propertyName)
            {
                case "Tests":
                    this.SaveAsTestSuiteCommand.Refresh();
                    break;
                case "Settings":
                    break;
                default:
                    break;
            }
        }

        internal void Load(string fullPath)
        {
            testSuiteManager.Open(fullPath, null, null);

            //var tests = testSuiteManager.GetTests();
            //View.Tests = new BindingList<Test>(tests.ToArray());

            //View.SettingsNames = new BindingList<string>(settingsManager.GetNames());

            
        }

        internal void Save(string fullPath)
        {
            testSuiteManager.DefineSettings(Settings);
            testSuiteManager.DefineTests(Tests);
            testSuiteManager.SaveAs(fullPath);
        }
    }
}
