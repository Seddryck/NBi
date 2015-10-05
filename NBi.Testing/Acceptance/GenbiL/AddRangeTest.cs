using System;
using System.IO;
using System.Linq;
using NBi.GenbiL;
using NUnit.Framework;
using System.Xml;
using NBi.Xml;

namespace NBi.Testing.Acceptance.GenbiL
{
    [TestFixture]
    public class AddRangeTest
    {
        private const string TEST_SUITE_NAME="AddRange-TestSuite";
        private string DefinitionFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".genbil"; } }
        private string TargetFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + ".nbits"; } }

        private string AddRangeFilename { get { return "Acceptance\\GenbiL\\Resources\\" + TEST_SUITE_NAME + "_ToAdd.nbits"; } }

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
        public void Run_AddRangeTestSuiteBuild_FileGenerated()
        {
            var generator = new TestSuiteGenerator();
            generator.Load(DefinitionFilename);
            generator.Execute();

            Assert.That(File.Exists(TargetFilename));

            int i = 0;
            var docXml = new XmlDocument();
            docXml.Load(TargetFilename);
            foreach (XmlNode node in docXml.DocumentElement.ChildNodes)
                if (node.Name=="test")
                    i++;
	
            Assert.That(i, Is.EqualTo(4));
        }

    }
}
