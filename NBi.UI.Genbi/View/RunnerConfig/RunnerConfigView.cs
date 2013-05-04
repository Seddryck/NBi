using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Interface.RunnerConfig;

namespace NBi.UI.Genbi.View.RunnerConfig
{
    public partial class RunnerConfigView : Form
    {
        internal RunnerConfigViewAdapter Adapter { get; set; }

        public RunnerConfigView(RunnerConfigViewAdapter adapter)
        {
            Adapter = adapter;
            InitializeComponent();
        }

        private void Apply_Click(object sender, EventArgs e)
        {
            var eventArgs = new RunnerConfigBuildEventArgs();
            eventArgs.RootPath = rootPath.Path;
            eventArgs.FrameworkPath = frameworkPath.Path;
            eventArgs.TestSuitePath = testSuiteFile.Path;
            eventArgs.IsNUnit = buildNUnit.Checked;
            eventArgs.IsGallio = buildGallio.Checked;

            Adapter.InvokeBuild(eventArgs);
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
