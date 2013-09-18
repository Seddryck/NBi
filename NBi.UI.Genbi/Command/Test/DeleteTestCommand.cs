using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command.Test
{
	class DeleteTestCommand: CommandBase
	{
		private readonly TestListPresenter presenter;

		public DeleteTestCommand(TestListPresenter presenter)
		{
			this.presenter = presenter;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.SelectedTests != null || presenter.SelectedTest != null;
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			if (!(presenter.SelectedTests != null || presenter.SelectedTest != null))
				throw new InvalidOperationException("No test selected. Impossible to delete it.");

			foreach (var test in presenter.SelectedTests)
				presenter.Manager.Remove(test);
			
			presenter.ReloadTests();
		}
	}
}
