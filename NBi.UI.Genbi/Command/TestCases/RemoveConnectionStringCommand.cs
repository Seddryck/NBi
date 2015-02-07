using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command.TestCases
{
    class RemoveConnectionStringCommand : CommandBase
    {
        protected readonly TestCasesPresenter presenter;

        public RemoveConnectionStringCommand(TestCasesPresenter presenter)
        {
            this.presenter = presenter;
        }

        /// <summary>
        /// Refreshes the command state.
        /// </summary>
        public override void Refresh()
        {
            this.IsEnabled = presenter.ConnectionStringNames!=null && presenter.ConnectionStringNames.Count > 0 && presenter.ConnectionStringSelectedIndex > -1;
        }

        /// <summary>
        /// Executes the command logics.
        /// </summary>
        public override void Invoke()
        {
            DialogResult result = MessageBox.Show("Are you sure you want to delete the selected connection-string?", "Remove connection-string", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
                presenter.State.ConnectionStrings.Remove(presenter.ConnectionStringSelectedName);
        }
    }
}
