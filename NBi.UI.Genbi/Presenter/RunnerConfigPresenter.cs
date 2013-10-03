using System;
using System.IO;
using System.Linq;
using NBi.Service.RunnerConfig;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Command.Configs;

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
            switch (propertyName)
            {
                case "RootPath":
                    if (CheckPath(propertyName, RootPath))
                        SetDefaultPaths();
                    break;
                case "FrameworkPath":
                    CheckPath(propertyName, FrameworkPath);
                    break;
                case "TestSuiteFile":
                    CheckFile(propertyName, TestSuiteFile);
                    break;
                default:
                    break;
            }
            CreateConfigsCommand.Refresh();
        }


        private bool CheckPath(string propertyName, string path)
        {
            if (!IsValidPath(path))
                SendWarning(propertyName, "Invalid");
            else
                SendValidation(propertyName);

            return IsValidPath(path);
        }

        private bool IsValidPath(string path)
        {
            return 
                string.IsNullOrEmpty(path) 
                || 
                (
                    path.Intersect(Path.GetInvalidPathChars()).Count() == 0 
                    && Path.IsPathRooted(path) 
                    && Directory.Exists(path)
                );
        }


        private void CheckFile(string propertyName, string file)
        {
            try
            {
                Path.GetDirectoryName(file);
            }
            catch (ArgumentException ex)
            {
                SendWarning(propertyName, "Invalid path for directory of");
                return;
            }


            if (Path.GetFileName(file).Length == 0)
                SendWarning(propertyName, "No filename given for");
            else if (Path.GetExtension(file) != ".nbits")
                SendWarning(propertyName, "Expected extension was 'nbits' for");
            else
                SendValidation(propertyName);
        }

        private void SetDefaultPaths()
        {
            if (string.IsNullOrEmpty(FrameworkPath) || !FrameworkPath.StartsWith(RootPath))
                FrameworkPath = RootPath;

            if (string.IsNullOrEmpty(TestSuiteFile) || !TestSuiteFile.StartsWith(RootPath))
            {
                if (string.IsNullOrEmpty(TestSuiteFile))
                    TestSuiteFile = RootPath;
                else
                    if (RootPath.EndsWith(Path.PathSeparator.ToString()))
                        TestSuiteFile = RootPath + Path.GetFileName(TestSuiteFile);
                    else
                        TestSuiteFile = RootPath + @"\" + Path.GetFileName(TestSuiteFile);
            }
        }


        public Action<string, string> SendWarning;
        public Action<string> SendValidation;

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
