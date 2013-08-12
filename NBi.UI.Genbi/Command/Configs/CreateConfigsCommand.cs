using System;
using System.Linq;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.Command.Configs
{
    class CreateConfigsCommand : CommandBase
    {
        private readonly RunnerConfigPresenter presenter;


        public CreateConfigsCommand(RunnerConfigPresenter presenter)
		{
			this.presenter = presenter;
		}

        public override void Invoke()
        {
            presenter.Create(presenter.FrameworkPath, presenter.RootPath, presenter.TestSuiteFile, presenter.IsBuildNUnit, presenter.IsBuildGallio);
        }

        public override void Refresh()
        {
            this.IsEnabled = !(string.IsNullOrEmpty(presenter.FrameworkPath)
                                || string.IsNullOrEmpty(presenter.RootPath)
                                || string.IsNullOrEmpty(presenter.TestSuiteFile))
                                && (presenter.IsBuildNUnit || presenter.IsBuildGallio);
        }
    }
}
