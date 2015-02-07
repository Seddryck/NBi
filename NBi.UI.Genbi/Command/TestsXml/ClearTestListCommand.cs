using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.Command.TestsXml
{
	class ClearTestListCommand: CommandBase
	{
		private readonly TestListPresenter presenter;

		public ClearTestListCommand(TestListPresenter presenter)
		{
			this.presenter = presenter;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = presenter.State.Suite.Tests.Count > 0;
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			var diagRes = MessageBox.Show(
				"Are your sure you want to clear the test-suite?",
				"Genbi",
				MessageBoxButtons.OKCancel,
				MessageBoxIcon.Question,
				MessageBoxDefaultButton.Button1);

			if (diagRes.HasFlag(DialogResult.OK))
			{
				presenter.Clear();
				ShowInform(String.Format("Test suite has been cleared."));
			}
			
		}
	}
}
