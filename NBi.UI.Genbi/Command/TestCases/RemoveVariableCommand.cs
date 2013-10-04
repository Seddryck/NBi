using System;
using System.Linq;
using NBi.UI.Genbi.Presenter;

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
			presenter.Remove(presenter.VariableSelectedIndex);
		}
	}
}
