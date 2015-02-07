using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command.TestCases
{
    class EditConnectionStringCommand : CommandBase
    {
        protected readonly TestCasesPresenter presenter;
        private readonly ConnectionStringWindow window;


        public EditConnectionStringCommand(TestCasesPresenter presenter, ConnectionStringWindow window)
        {
            this.presenter = presenter;
            this.window = window;
        }

        /// <summary>
        /// Refreshes the command state.
        /// </summary>
        public override void Refresh()
        {
            this.IsEnabled = presenter.ConnectionStringNames != null && presenter.ConnectionStringNames.Count>0 && presenter.ConnectionStringSelectedIndex>-1;
        }

        /// <summary>
        /// Executes the command logics.
        /// </summary>
        public override void Invoke()
        {
            window.IsNameEditable = false;
            window.NameId = presenter.ConnectionStringSelectedName;
            window.Value = presenter.ConnectionStringSelectedValue;
            DialogResult result = window.ShowDialog();
            if (result == DialogResult.OK)
                presenter.State.ConnectionStrings.Edit(window.NameId, window.Value);
            
        }
    }
}
