using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.GenbiL;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.GenbiL
{
    [TestFixture]
    public class ExecutionEqualTo
    {
        private const string TEST_SUITE_NAME = "ExecutionEqualTo";
        private string DefinitionFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".genbil"; } }
        private string TargetFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".nbits"; } }
        private string CsvFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".csv"; } }

        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
        public void TearDownMethods()
        {

        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
            if (File.Exists(TargetFilename))
                File.Delete(TargetFilename);

            //if(File.Exists(CsvFilename))
            //    File.Delete(CsvFilename);
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
            if (File.Exists(TargetFilename))
                File.Delete(TargetFilename);
        }
        #endregion

        [Test]
        public void Execute_GenbiL_FileGenerated()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            Assert.That(File.Exists(TargetFilename));
        }

        [Test]
        public void Execute_GenbiL_EqualToIsThere()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            if (!File.Exists(TargetFilename))
                Assert.Inconclusive("Test Suite not generated!");

            var content = File.ReadAllText(TargetFilename);
            Console.WriteLine(content);
            Assert.That(content, Does.Contain("<query connection-string=\"@tst\" timeout-milliSeconds=\"10000\">"));
            Assert.That(content, Does.Contain("<query connection-string=\"@tst\" timeout-milliSeconds=\"5000\">"));
            Assert.That(content, Does.Contain("<equal-to>"));
            Assert.That(content, Does.Contain("TC02"));
        }
    }
}
