using System;
using System.IO;
using System.Linq;
using NBi.GenbiL;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.GenbiL
{
    [TestFixture]
    public class syntaxOldNewTest
    {
        private const string TEST_SUITE_NAME= "SyntaxOldNew";
        private string DefinitionFilename { get => $"Acceptance\\GenbiL\\Resources\\{TEST_SUITE_NAME}.genbil"; }
        private string TargetFilename { get => $"Acceptance\\GenbiL\\Resources\\{TEST_SUITE_NAME}.nbits"; }

        private string ExpectedFilename { get => $"Acceptance\\GenbiL\\Resources\\{TEST_SUITE_NAME}.expected.nbits"; }
        private string CsvFilename { get => $"Acceptance\\GenbiL\\Resources\\{TEST_SUITE_NAME}.csv"; }

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
        public void Execute_SyntaxOldNew_FileGenerated()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            Assert.That(File.Exists(TargetFilename));
        }

        [Test]
        public void Execute_SyntaxOldNew_MatchWithExpectations()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            if (!File.Exists(TargetFilename))
                Assert.Inconclusive("Test Suite not generated!");

            var content = File.ReadAllText(TargetFilename);
            var expected = File.ReadAllText(ExpectedFilename);
            Assert.That(content, Is.EqualTo(expected));
        }

    }
}
