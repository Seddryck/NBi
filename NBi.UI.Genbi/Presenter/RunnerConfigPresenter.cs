using System;
using System.Linq;
using NBi.Service.RunnerConfig;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.Configs;
using NBi.UI.Genbi.Interface.RunnerConfig;
using NBi.UI.Genbi.Stateful;

namespace NBi.UI.Genbi.Presenter
{
    class RunnerConfigPresenter : PresenterBase
    {
        private readonly RunnerConfigManager runnerConfigManager;

        public RunnerConfigPresenter(RunnerConfigManager runnerConfigManager)
            : base()
        {
            this.runnerConfigManager = runnerConfigManager;

            CreateConfigsCommand = new CreateConfigsCommand(this);
        }

        public ICommand CreateConfigsCommand { get; private set; }

        #region Bindable properties

       
        public string FrameworkPath
        {
            get { return GetValue<string>("FrameworkPath"); }
            set { SetValue("FrameworkPath", value); }
        }

        public string RootPath
        {
            get { return GetValue<string>("RootPath"); }
            set { SetValue("RootPath", value); }
        }

        public string TestSuiteFile
        {
            get { return GetValue<string>("TestSuiteFile"); }
            set { SetValue("TestSuiteFile", value); }
        }

        public bool IsBuildNUnit
        {
            get { return GetValue<bool>("IsBuildNUnit"); }
            set { SetValue("IsBuildNUnit", value); }
        }
        
        public bool IsBuildGallio
        {
            get { return GetValue<bool>("IsBuildGallio"); }
            set { SetValue("IsBuildGallio", value); }
        }
        
        #endregion

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);
            //TODO When properties are changed, call commands refresh
            switch (propertyName)
            {
                case "State":
                    break;
                default:
                    break;
            }
            CreateConfigsCommand.Refresh();
        }

        internal void Create(string frameworkPath, string rootPath, string testSuitePath, bool isNUnit, bool isGallio)
        {
            runnerConfigManager.Build(
                rootPath,
                frameworkPath,
                testSuitePath,
                isNUnit,
                isGallio
                );
        }

    }
}
