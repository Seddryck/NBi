using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;
using NBi.GenbiL.Action.Case;

namespace NBi.UI.Genbi.Command.TestCases
{
    class RunQueryCommand : CommandBase
    {
        protected readonly TestCasesPresenter presenter;

        public RunQueryCommand(TestCasesPresenter presenter)
        {
            this.presenter = presenter;
        }

        /// <summary>
        /// Refreshes the command state.
        /// </summary>
        public override void Refresh()
        {
            this.IsEnabled = !string.IsNullOrEmpty(presenter.Query) && presenter.ConnectionStringSelectedIndex>-1;
        }

        /// <summary>
        /// Executes the command logics.
        /// </summary>
        public override void Invoke()
        {
            //TODO get the connStrig
            var loadQuery = new LoadFromQueryCaseAction(presenter.Query, "");
            loadQuery.Execute(presenter.State);
            ShowInform("Query executed and test-cases loaded!");
        }
    }
}
