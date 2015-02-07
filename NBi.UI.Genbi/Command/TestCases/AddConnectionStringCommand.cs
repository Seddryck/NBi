using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi.Command.TestCases
{
    class AddConnectionStringCommand : CommandBase
    {
        protected readonly TestCasesPresenter presenter;
        private readonly ConnectionStringWindow window;


        public AddConnectionStringCommand(TestCasesPresenter presenter, ConnectionStringWindow window)
        {
            this.presenter = presenter;
            this.window = window;
        }

        /// <summary>
        /// Refreshes the command state.
        /// </summary>
        public override void Refresh()
        {
            this.IsEnabled = true;
        }

        /// <summary>
        /// Executes the command logics.
        /// </summary>
        public override void Invoke()
        {
            window.IsNameEditable = true;
            if (string.IsNullOrEmpty(window.NameId))
                window.NameId="default";
            DialogResult result = window.ShowDialog();
            if (result == DialogResult.OK)
                presenter.State.ConnectionStrings.Add(window.NameId, window.Value);
        }
    }
}
