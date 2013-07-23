using System;
using System.Linq;
using NBi.Service.RunnerConfig;
using NBi.UI.Genbi.Interface.RunnerConfig;

namespace NBi.UI.Genbi.Presenter
{
    public class RunnerConfigPresenter : PresenterBase
    {
        private readonly RunnerConfigManager runnerConfigManager;

        public RunnerConfigPresenter(IRunnerConfigView view)
        {
            runnerConfigManager = new RunnerConfigManager();
        }

        //protected override void OnViewInitialize(object sender, EventArgs e)
        //{
        //    base.OnViewInitialize(sender, e);
        //    Initialize();

        //    View.Build += OnBuild;
        //}

        //private void OnBuild(object sender, RunnerConfigBuildEventArgs e)
        //{
        //    runnerConfigManager.Build(
        //        e.RootPath,
        //        e.FrameworkPath,
        //        e.TestSuitePath,
        //        e.IsNUnit,
        //        e.IsGallio
        //        );
        //}

        protected void Initialize()
        {
            
        }

    }
}
