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

        public TestSuitePresenter(ITestSuiteView testSuiteView, TestSuiteManager testSuiteManager, BindingList<Test> tests, BindingList<Setting> settings)
            : base(testSuiteView)
        {
            this.testSuiteManager = testSuiteManager;


            this.OpenTestSuiteCommand = new OpenTestSuiteCommand(this);
            this.SaveAsTestSuiteCommand = new SaveAsTestSuiteCommand(this);
            this.Tests = tests;
            this.Settings = settings;
        }

        public ICommand OpenTestSuiteCommand { get; private set; }
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
            testSuiteManager.Open(fullPath);

            Tests.Clear();
            foreach (var test in testSuiteManager.GetTests())
                Tests.Add(test);

            Settings.Clear();
            foreach (var setting in testSuiteManager.GetSettings())
                Settings.Add(setting);

            this.SaveAsTestSuiteCommand.Refresh();
            OnTestSuiteLoaded(EventArgs.Empty);
        }

        internal void Save(string fullPath)
        {
            testSuiteManager.DefineSettings(Settings);
            testSuiteManager.DefineTests(Tests);
            testSuiteManager.SaveAs(fullPath);
        }

        public event EventHandler<EventArgs> TestSuiteLoaded;

        public void OnTestSuiteLoaded(EventArgs e)
        {
            EventHandler<EventArgs> handler = TestSuiteLoaded;
            if (handler != null)
                handler(this, e);
        }

        internal void RefreshCommands()
        {
            this.SaveAsTestSuiteCommand.Refresh();
            this.OpenTestSuiteCommand.Refresh();
        }
    }
}
