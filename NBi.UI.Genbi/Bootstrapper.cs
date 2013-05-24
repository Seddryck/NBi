using System;
using System.Linq;
using System.Windows.Forms;
using NBi.UI.Genbi.View;

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

            //var transformationService = new TransformationService();
            //var validationService = new ValidationService();
            //var documentService = new DocumentService();
            //var documentPresenterFactory = new DocumentPresenterFactory(documentService);
            //var interactionManager = new InteractionManager();

            //var masterPresenter = new MasterPresenter(documentPresenterFactory, validationService, transformationService, interactionManager);
            var dispatcher = new GenbiDispatcher();
            dispatcher.Initialize();


            if (args != null && args.Length != 0)
            {
                var testSuiteToOpen = args[0];
                //TODO dispatcher.Open(testSuiteToOpen);
            }

            Application.Run(dispatcher.GetMainForm());
        }
    }
}
