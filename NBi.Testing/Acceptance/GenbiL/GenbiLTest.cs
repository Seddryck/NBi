using System;
using System.IO;
using System.Linq;
using NBi.genbiL;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.GenbiL
{
    


    [TestFixture]
    public class GenbiLTest
    {
        private const string TEST_SUITE_NAME="Simple-TestSuiteBuild";
        private string DefintionFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".xml"; } }
        private string TargetFilename { get { return TEST_SUITE_NAME + ".nbits"; } }

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
            generator.Load(DefintionFilename);
            generator.Execute();

            Assert.That(File.Exists(TargetFilename));
        }

        [Test]
        public void Run_SimpleTestSuiteBuild_FileIsCorrect()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefintionFilename);
            generator.Execute();

            if (!File.Exists(TargetFilename))
                Assert.Inconclusive("Test Suite not generated!");

            var content = File.ReadAllText(TargetFilename);
            Assert.That(content, Is.StringContaining("<dimension"));
            Assert.That(content, Is.StringContaining("caption=\"first-dimension\""));
            Assert.That(content, Is.StringContaining("caption=\"second-dimension\""));
            Assert.That(content, Is.StringContaining("perspective=\"first-perspective\""));
            Assert.That(content, Is.StringContaining("exist"));
        }
    }
}
