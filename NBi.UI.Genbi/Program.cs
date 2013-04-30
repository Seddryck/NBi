using System;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter.Generator;
using NBi.UI.Genbi.View.Generator;

namespace NBi.UI.Genbi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var adapter = new TestSuiteViewAdapter();
            var presenter = new GenbiPresenter(adapter);
            adapter.InvokeInitialize(EventArgs.Empty);

            Application.Run(adapter.MainForm);
        }
    }
}
