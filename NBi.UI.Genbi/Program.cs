using System;
using System.Threading;
using System.Windows.Forms;
using NBi.UI.Genbi.View;

namespace NBi.UI.Genbi
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(params string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.ThreadException += OnUnhandledException;

            var bootstrapper = new Bootstrapper();
            bootstrapper.Boot(args);
        }


        private static void OnUnhandledException(object sender, ThreadExceptionEventArgs e)
        {
            var window = new ExceptionManagerWindow(e.Exception);
            window.Show();
        }
    }
}
