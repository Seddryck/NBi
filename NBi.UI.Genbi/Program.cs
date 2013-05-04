using System;
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
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var dispatcher = new GenbiDispatcher();
            dispatcher.Initialize();

            Application.Run(dispatcher.GetMainForm());
        }
    }
}
