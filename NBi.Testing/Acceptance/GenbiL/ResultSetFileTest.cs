using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NBi.GenbiL;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.GenbiL
{
    [TestFixture]
    public class ResultSetFileTest
    {
        private const string TEST_SUITE_NAME="ResultSetFile";
        private string DefinitionFilename { get => $"Acceptance\\GenbiL\\Resources\\{TEST_SUITE_NAME}.genbil"; }
        private string TargetFilename { get => $"Acceptance\\GenbiL\\Resources\\{TEST_SUITE_NAME}.nbits"; }
        private string ExpectedFilename { get => $"Acceptance\\GenbiL\\Resources\\{TEST_SUITE_NAME}.expected.nbits"; }

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
        public void Execute_Group_FileGenerated()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            Assert.That(File.Exists(TargetFilename));
        }

        [Test]
        public void Execute_Group_ExpectedContent()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            var expected = File.ReadAllText(ExpectedFilename);
            var actual = File.ReadAllText(TargetFilename);
            actual = Regex.Replace(actual, @"(\s*)<edition(.*)/>", "", RegexOptions.Multiline);
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
