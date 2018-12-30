using System;
using System.IO;
using System.Linq;
using NBi.GenbiL;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.GenbiL
{
    [TestFixture]
    public class DictionaryTest
    {
        private const string TEST_SUITE_NAME= "Dictionary";
        private string DefinitionFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".genbil"; } }
        private string TargetFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".nbits"; } }
        private string CsvFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".csv"; } }

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
        public void Execute_Dictionary_FileGenerated()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            Assert.That(File.Exists(TargetFilename));
        }

        [Test]
        public void Execute_Dictionary_Set()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            if (!File.Exists(TargetFilename))
                Assert.Inconclusive("Test Suite not generated!");

            var content = File.ReadAllText(TargetFilename);
            Assert.That(content, Is.StringContaining("<predicate operand=\"[foo]\">"));
            Assert.That(content, Is.StringContaining("<equal>0</equal>"));
            Assert.That(content, Is.StringContaining("<predicate operand=\"[bar]\">"));
            Assert.That(content, Is.StringContaining("<equal>None</equal>"));

            Assert.That(content, Is.StringContaining("<equal>1</equal>"));
            Assert.That(content, Is.StringContaining("<equal>Some</equal>"));
            Assert.That(content, Is.StringContaining("<predicate operand=\"[wax]\">"));
            Assert.That(content, Is.StringContaining("<equal>false</equal>"));
        }
    }
}
