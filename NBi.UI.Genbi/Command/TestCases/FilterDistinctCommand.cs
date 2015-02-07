using NBi.GenbiL.Action.Case;
using NBi.UI.Genbi.Presenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.UI.Genbi.Command.TestCases
{
    class FilterDistinctCommand : CommandBase
    {
        protected readonly TestCasesPresenter presenter;

        public FilterDistinctCommand(TestCasesPresenter presenter)
        {
            this.presenter = presenter;
        }

        /// <summary>
        /// Refreshes the command state.
        /// </summary>
        public override void Refresh()
        {
            this.IsEnabled = presenter.TestCases.Rows.Count>0;
        }

        /// <summary>
        /// Executes the command logics.
        /// </summary>
        public override void Invoke()
        {
            var filterDistinct = new FilterDistinctCaseAction();
            filterDistinct.Execute(presenter.State);
        }
    }
}
