using System;
using System.Linq;
using NBi.UI.Genbi.Interface;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.Command.TestsXml
{
	class GenerateTestListCommand: CommandBase
	{
		private readonly TestListPresenter presenter;

		public GenerateTestListCommand(TestListPresenter presenter)
		{
			this.presenter = presenter;
		}

		/// <summary>
		/// Refreshes the command state.
		/// </summary>
		public override void Refresh()
		{
			this.IsEnabled = !string.IsNullOrEmpty(presenter.Template) && presenter.TestCases.Rows.Count > 0;
		}

		/// <summary>
		/// Executes the command logics.
		/// </summary>
		public override void Invoke()
		{
			//var message = presenter.State.Suite.();

            //if (message.IsSuccess)
            //    ShowInform(message.Message);
            //else
            //    ShowException(message.Message);
				
		}
	}
}
