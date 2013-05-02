using System;
using System.Windows.Forms;
using NBi.UI.Genbi.Presenter;
using NBi.UI.Genbi.View.TestSuiteGenerator;

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
            var presenter = new TestSuiteGeneratorPresenter(adapter);
            adapter.InvokeInitialize(EventArgs.Empty);

            Application.Run(adapter.MainForm);
        }
    }
}
