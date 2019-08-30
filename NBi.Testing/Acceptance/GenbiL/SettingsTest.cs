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
            Assert.That(content, Is.StringContaining("<settings>"));
            

            Assert.That(content, Is.StringContaining("<default "));
            Assert.That(content, Is.StringContaining("apply-to=\"system-under-test\""));
            Assert.That(content, Is.StringContaining("<connection-string>myDefaultConnectionString</connection-string>"));

            Assert.That(content, Is.StringContaining("<reference"));
            Assert.That(content, Is.StringContaining("name=\"olap\""));
            Assert.That(content, Is.StringContaining("<connection-string>myReferenceConnectionString</connection-string>"));

            Assert.That(content, Is.StringContaining("<regex"));
            Assert.That(content, Is.StringContaining("[AZ-az]"));

            Assert.That(content, Is.StringContaining("<csv-profile"));
            Assert.That(content, Is.StringContaining("field-separator=\"Tab\""));
            Assert.That(content, Is.Not.StringContaining("<MissingCell"));
            Assert.That(content, Is.Not.StringContaining("<EmptyCell"));
        }

    }
}
