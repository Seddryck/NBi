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
            Assert.That(content, Is.StringContaining("<dimension "));
            Assert.That(content, Is.StringContaining("caption=\"first-dimension\""));
            Assert.That(content, Is.StringContaining("caption=\"second-dimension\""));
            Assert.That(content, Is.Not.StringContaining("caption=\"exclude-dimension\""));
            Assert.That(content, Is.StringContaining("perspective=\"first-perspective\""));
            Assert.That(content, Is.StringContaining("exist"));

            Assert.That(content, Is.StringContaining("<dimensions "));
            Assert.That(content, Is.StringContaining("<subsetOf"));

            Assert.That(content, Is.StringContaining("<default"));
            Assert.That(content, Is.StringContaining("apply-to=\"assert\""));
            Assert.That(content, Is.StringContaining("<connectionString>youyou-default-assert</connectionString>"));

            Assert.That(content, Is.StringContaining("<default"));
            Assert.That(content, Is.StringContaining("apply-to=\"system-under-test\""));
            Assert.That(content, Is.StringContaining("<connectionString>youyou-default-sut</connectionString>"));
            Assert.That(content, Is.Not.StringContaining("name=\"System-Under-Test\""));
            Assert.That(content, Is.Not.StringContaining("<report />"));

            Assert.That(content, Is.StringContaining("<reference"));
            Assert.That(content, Is.StringContaining("name=\"noway\""));
            Assert.That(content, Is.StringContaining("<connectionString>youyou-reference-noway</connectionString>"));

            Assert.That(content, Is.StringContaining("<hierarchy "));
            Assert.That(content, Is.StringContaining("caption=\"first-hierarchy\""));
            Assert.That(content, Is.StringContaining("caption=\"second-hierarchy\""));
            Assert.That(content, Is.Not.StringContaining("caption=\"third-hierarchy\""));
            Assert.That(content, Is.StringContaining("dimension=\"first-dimension\""));

            Assert.That(content, Is.StringContaining("<hierarchies "));
            Assert.That(content, Is.StringContaining("<subsetOf"));
            Assert.That(content, Is.Not.StringContaining("<item>fourth-hierarchy</item>"));

            //Assert.That(content, Is.StringContaining("<parallelize-queries>false</parallelize-queries>"));
        }

    }
}
