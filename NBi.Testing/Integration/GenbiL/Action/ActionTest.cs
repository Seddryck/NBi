using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Template;
using NBi.GenbiL.Action.Suite;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NBi.GenbiL.Action;
using NBi.GenbiL.Stateful;

namespace NBi.Testing.Integration.GenbiL.Action
{
    public class ActionTest
    {
        private const string TEST_SUITE_NAME = "action";

        private string TargetFilename { get { return "Integration\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".nbits"; } }
        private string CsvFilename { get { return "Integration\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".csv"; } }

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {

        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
            if (File.Exists(TargetFilename))
                File.Delete(TargetFilename);
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
            //if (File.Exists(TargetFilename))
            //    File.Delete(TargetFilename);
        }
        #endregion

        [Test]
        public void Execute_ManyActions_SuiteGenerated()
        {
            var state = new GenerationState();

            var loadCase = new LoadCaseFromFileAction(CsvFilename);
            loadCase.Execute(state);

            var loadTemplate = new LoadEmbeddedTemplateAction("ExistsDimension");
            loadTemplate.Execute(state);

            var generateSuite = new GenerateSuiteAction(false);
            generateSuite.Execute(state);

            var saveSuite = new SaveSuiteAction(TargetFilename);
            saveSuite.Execute(state);

            Assert.That(File.Exists(TargetFilename));
        }

        [Test]
        public void Execute_ManyActions2_SuiteGenerated()
        {
            var state = new GenerationState();

            var actions = new List<IAction>()
            {
                new LoadCaseFromFileAction(CsvFilename)
                , new LoadEmbeddedTemplateAction("ExistsDimension")
                , new GenerateSuiteAction(false)
                , new SaveSuiteAction(TargetFilename)
            };

            actions.ForEach(a => a.Execute(state));
            
            Assert.That(File.Exists(TargetFilename));
        }
    }
}
