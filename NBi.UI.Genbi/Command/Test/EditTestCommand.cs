using System;
using System.Linq;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command.Test
{
	class EditTestCommand: CommandBase
	{
		private readonly TestListPresenter presenter;
		private readonly DisplayTestView view;


		public EditTestCommand(TestListPresenter presenter, DisplayTestView displayTestView)
		{
			this.presenter = presenter;
			view = displayTestView;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.SelectedTest != null && presenter.SelectedTests!=null && presenter.SelectedTests.Count() == 1;
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			view.Test = presenter.SelectedTest;
			view.DeclareBindings();
			view.Show();
		}
	}
}
