using System;
using System.Linq;
using NBi.UI.Genbi.Presenter;

namespace NBi.UI.Genbi.Command.TestsXml
{
	class UndoGenerateTestListCommand : CommandBase
	{
		private readonly TestListPresenter presenter;

		public UndoGenerateTestListCommand(TestListPresenter presenter)
		{
			this.presenter = presenter;
		}

        public override void Refresh()
        {
            this.IsEnabled = presenter.IsUndo;
        }

        /// <summary>
        /// Executes the command logics.
        /// </summary>
        public override void Invoke()
        {
            presenter.Undo();
        }

	}
}
