using System;
using System.IO;
using System.Linq;
using NBi.GenbiL;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.GenbiL
{
    [TestFixture]
    public class GenbiLTest
    {
        private const string TEST_SUITE_NAME="Simple-TestSuiteBuild";
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
            Assert.That(content, Does.Contain("<dimension "));
            Assert.That(content, Does.Contain("caption=\"first-dimension\""));
            Assert.That(content, Does.Contain("caption=\"second-dimension\""));
            Assert.That(content, Is.Not.StringContaining("caption=\"exclude-dimension\""));
            Assert.That(content, Does.Contain("perspective=\"first-perspective\""));
            Assert.That(content, Does.Contain("exist"));

            Assert.That(content, Does.Contain("<dimensions "));
            Assert.That(content, Does.Contain("<contained-in"));

            Assert.That(content, Does.Contain("<default"));
            Assert.That(content, Does.Contain("apply-to=\"assert\""));
            Assert.That(content, Does.Contain("<connection-string>youyou-default-assert</connection-string>"));

            Assert.That(content, Does.Contain("<default"));
            Assert.That(content, Does.Contain("apply-to=\"system-under-test\""));
            Assert.That(content, Does.Contain("<connection-string>youyou-default-sut</connection-string>"));
            Assert.That(content, Is.Not.StringContaining("name=\"System-Under-Test\""));
            Assert.That(content, Is.Not.StringContaining("<report />"));

            Assert.That(content, Does.Contain("<reference"));
            Assert.That(content, Does.Contain("name=\"noway\""));
            Assert.That(content, Does.Contain("<connection-string>youyou-reference-noway</connection-string>"));

            Assert.That(content, Does.Contain("<hierarchy "));
            Assert.That(content, Does.Contain("caption=\"first-hierarchy\""));
            Assert.That(content, Does.Contain("caption=\"second-hierarchy\""));
            Assert.That(content, Is.Not.StringContaining("caption=\"third-hierarchy\""));
            Assert.That(content, Does.Contain("dimension=\"first-dimension\""));

            Assert.That(content, Does.Contain("<hierarchies "));
            Assert.That(content, Does.Contain("<contained-in"));
            Assert.That(content, Is.Not.StringContaining("<item>fourth-hierarchy</item>"));

            Assert.That(content, Does.Contain("<parallelize-queries>false</parallelize-queries>"));
        }

    }
}
