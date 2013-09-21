using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Command;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.Stateful;

namespace NBi.UI.Genbi.View.RunnerConfig
{
    public partial class RunnerConfigView : Form
    {
        private RunnerConfigPresenter RunnerConfigPresenter { get; set; }

        public RunnerConfigView()
        {
            RunnerConfigPresenter = new RunnerConfigPresenter(new Service.RunnerConfig.RunnerConfigManager());
            
            InitializeComponent();
            DeclareBindings();
            BindPresenter();
        }

        protected void DeclareBindings()
        {
            DataBind(RunnerConfigPresenter);
        }

        private void BindPresenter()
        {
            //TestCases & Variables
            CommandManager.Instance.Bindings.Add(this.RunnerConfigPresenter.CreateConfigsCommand, apply);
        }

        internal void DataBind(RunnerConfigPresenter presenter)
        {
            if (presenter != null)
            {
                frameworkPath.DataBindings.Add("Path", presenter, "FrameworkPath", false, DataSourceUpdateMode.OnPropertyChanged);
                rootPath.DataBindings.Add("Path", presenter, "RootPath", false, DataSourceUpdateMode.OnPropertyChanged);
                testSuiteFile.DataBindings.Add("Path", presenter, "TestSuiteFile", false, DataSourceUpdateMode.OnPropertyChanged);
                buildNUnit.DataBindings.Add("Checked", presenter, "IsBuildNUnit", false, DataSourceUpdateMode.OnPropertyChanged);
                buildGallio.DataBindings.Add("Checked", presenter, "IsBuildGallio", false, DataSourceUpdateMode.OnPropertyChanged);
            }
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void RunnerConfigView_Load(object sender, EventArgs e)
        {

        }
    }
}
