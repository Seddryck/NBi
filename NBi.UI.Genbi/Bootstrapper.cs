using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.View.RunnerConfig;
using NBi.UI.Genbi.View.TestSuiteGenerator;

namespace NBi.UI.Genbi
{
    class Bootstrapper
    {
        /// <summary>
        /// Boots the application.
        /// </summary>
        /// <param name="args">
        /// Parameters for the application startup.
        /// </param>
        public void Boot(params string[] args)
        {
            var masterView = new TestSuiteView();

            if (args != null && args.Length != 0)
            {
                var testSuiteToOpen = args[0];
                //TODO dispatcher.Open(testSuiteToOpen);
            }

            Application.Run(masterView);
        }
    }
}
