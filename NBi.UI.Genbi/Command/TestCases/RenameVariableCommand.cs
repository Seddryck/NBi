using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;
using NBi.GenbiL.Action.Case;

namespace NBi.UI.Genbi.Command.TestCases
{
	class RenameVariableCommand : CommandBase
	{
		private readonly TestCasesPresenter presenter;
		private readonly RenameVariableWindow window;

		public RenameVariableCommand(TestCasesPresenter presenter, RenameVariableWindow window)
		{
			this.presenter = presenter;
			this.window = window;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.IsRenamable();
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			DialogResult result = window.ShowDialog();
			if (result == DialogResult.OK)
            {
                var index = presenter.VariableSelectedIndex;
                var rename = new RenameCaseAction(presenter.State.TestCaseSetCollection.Scope.Variables[index], window.NewName);
                rename.Execute(presenter.State);
            }
		}
	}
}
