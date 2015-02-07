using System;
using System.Diagnostics;
using System.Linq;
using NBi.UI.Genbi.Presenter;

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

			Debug.WriteLine("{0} elements to remove", presenter.SelectedTests.Count());
            //foreach (var test in presenter.SelectedTests)
            //    presenter.Manager.Remove(test);
			
			presenter.ReloadTests();
		}
	}
}
