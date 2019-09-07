using System;
using System.IO;
using System.Linq;
using NBi.GenbiL;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.GenbiL
{
    [TestFixture]
    public class SettingsTest
    {
        private const string TEST_SUITE_NAME="Settings";
        private string DefinitionFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".genbil"; } }
        private string TargetFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".nbits"; } }

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
        public void Run_SimpleTestSuiteBuild_FileGenerated()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            Assert.That(File.Exists(TargetFilename));
        }

        [Test]
        public void Run_SimpleTestSuiteBuild_FileIsCorrect()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            if (!File.Exists(TargetFilename))
                Assert.Inconclusive("Test Suite not generated!");

            var content = File.ReadAllText(TargetFilename);
            Assert.That(content, Does.Contain("<settings>"));
            

            Assert.That(content, Does.Contain("<default "));
            Assert.That(content, Does.Contain("apply-to=\"system-under-test\""));
            Assert.That(content, Does.Contain("<connection-string>myDefaultConnectionString</connection-string>"));

            Assert.That(content, Does.Contain("<reference"));
            Assert.That(content, Does.Contain("name=\"olap\""));
            Assert.That(content, Does.Contain("<connection-string>myReferenceConnectionString</connection-string>"));

            Assert.That(content, Does.Contain("<regex"));
            Assert.That(content, Does.Contain("[AZ-az]"));

            Assert.That(content, Does.Contain("<csv-profile"));
            Assert.That(content, Does.Contain("field-separator=\"Tab\""));
            Assert.That(content, Is.Not.StringContaining("<MissingCell"));
            Assert.That(content, Is.Not.StringContaining("<EmptyCell"));
        }

    }
}
