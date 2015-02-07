using System;
using System.Linq;
using NBi.UI.Genbi.Presenter;
using NBi.GenbiL.Action.Case;

namespace NBi.UI.Genbi.Command.TestCases
{
	class RemoveVariableCommand : CommandBase
	{
		private readonly TestCasesPresenter presenter;

		public RemoveVariableCommand(TestCasesPresenter presenter)
		{
			this.presenter = presenter;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.IsDeletable();
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			var index = presenter.VariableSelectedIndex;
            var remove = new RemoveCaseAction(presenter.State.TestCaseSetCollection.Scope.Variables[index]);
            remove.Execute(presenter.State);
		}
	}
}
