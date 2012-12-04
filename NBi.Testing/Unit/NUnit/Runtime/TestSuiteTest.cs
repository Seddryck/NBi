#region Using directives
using System;
using System.Linq;
using Moq;
using NBi.NUnit.Runtime;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.NUnit.Runtime
{
    [TestFixture]
    public class TestSuiteTest
    {

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
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        [Test]
        public void GetOwnFilename_DefaultValue_ReturnNBiNUnitRuntimedll()
        {
            //Buiding object used during test
            var testSuite = new TestSuite();
            
            //Call the method to test
            var filename = testSuite.GetOwnFilename();

            //Assertion
            Assert.That(filename, Is.EqualTo("NBi.NUnit.Runtime.dll"));
        }

        [Test]
        public void GetSelfFilename_DefaultValue_ReturnNBiNUnitRuntimedll()
        {
            //Buiding object used during test
            var testSuite = new TestSuite();

            //Call the method to test
            var name = testSuite.GetManifestName();

            //Assertion
            Assert.That(name, Is.EqualTo("NBi.NUnit.Runtime.dll"));
        }

        [Test]
        public void GetTestCases_TestCaseWithRegexName_ReplaceRegexByValueInName()
        {          
            //Buiding object used during test
            //TestSuite invoked
            var test = new TestXml()
            {
                Name = "my name contains a regex '{sut:caption}' and it's parsed! Same for '{sut:display-folder}'.",
            };

            test.Systems.Add(
                new StructureXml()
                {
                    Item = new MeasureXml()
                    {
                        Caption="My Caption",
                        DisplayFolder = "My Display Folder"
                    }
                }
            );

            var testSuiteXml = new TestSuiteXml();
            testSuiteXml.Tests.Add(test);
                    
            //Building a stub for TestSuiteManager
            var testSuiteManagerStub = new Mock<XmlManager>();
            testSuiteManagerStub.Setup(mgr => mgr.Load(It.IsAny<string>()));
            testSuiteManagerStub.Setup(mgr => mgr.TestSuite).Returns(testSuiteXml);

            var testSuite = new TestSuite(testSuiteManagerStub.Object);

            //Call the method to test
            var testCases = testSuite.GetTestCases();
            var testCase = testCases.First();

            //Assertion
            Console.WriteLine(testCase.TestName);
            Assert.That(testCase.TestName, Is.StringContaining("my name contains a regex").And
                                            .StringContaining("My Caption").And
                                            .StringContaining("My Display Folder").And
                                            .StringContaining("and it's parsed!"));
        }

    }
}
