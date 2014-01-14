using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command.Test
{
	class EditTestCommand: CommandBase
	{
		private readonly TestListPresenter presenter;
		//private readonly ViewTestWindow window;


		public EditTestCommand(TestListPresenter presenter)//, OpenTemplateWindow window)
		{
			this.presenter = presenter;
			//this.window = window;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.SelectedTest != null;
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			
		}
	}
}
