using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;
using NBi.GenbiL.Action.Case;
using System.Collections.Generic;

namespace NBi.UI.Genbi.Command.TestCases
{
    class FilterCommand : CommandBase
    {
        protected readonly TestCasesPresenter presenter;
        private readonly FilterWindow window;


        public FilterCommand(TestCasesPresenter presenter, FilterWindow window)
        {
            this.presenter = presenter;
            this.window = window;
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
            DialogResult result = window.ShowDialog();
            if (result == DialogResult.OK)
            {
                var index = presenter.VariableSelectedIndex;
                var variable = presenter.State.TestCaseSetCollection.Scope.Variables[index];
                var filterValues = new List<string>();
                filterValues.Add(window.FilterText);
                var filter = new FilterCaseAction(variable, window.Operator, filterValues, window.Negation);
                filter.Execute(presenter.State);
            }
        }
    }
}
