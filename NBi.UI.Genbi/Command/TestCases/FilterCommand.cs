using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

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
                presenter.Filter(presenter.VariableSelectedIndex, window.Operator, window.Negation, window.FilterText);
        }
    }
}
