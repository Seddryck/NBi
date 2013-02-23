#region Using directives
using System;
using System.Linq;
using Moq;
using NBi.Core;
using NBi.NUnit.Runtime;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using NUnit.Framework.Constraints;
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

            //Building a stub for TestSuiteFinder
            var testSuiteFinderStub = new Mock<TestSuiteFinder>();
            testSuiteFinderStub.Setup(finder => finder.Find()).Returns(string.Empty);

            var testSuite = new TestSuite(testSuiteManagerStub.Object, testSuiteFinderStub.Object);

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

        [Test]
        public void AssertTestCase_TestCaseFailing_StackTraceIsFilledWithXml()
        {
            var sut = "not empty";
            var ctr = new EmptyConstraint();
            var xmlContent = "<test><system></system><assert></assert></test>";

            var testSuite = new TestSuite();

            try
            {
                testSuite.AssertTestCase(sut, ctr, xmlContent);
            }
            catch (AssertionException ex)
            {
                Assert.That(ex.StackTrace, Is.EqualTo(xmlContent));
            }
            catch (Exception ex)
            {
                Assert.Fail("The exception should have been an AssertionException but was {0}.", new object[] { ex.GetType().FullName });
            }
        }

        [Test]
        public void AssertTestCase_TestCaseFailing_MessageIsAvailable()
        {
            var sut = "not empty string";
            var ctr = new EmptyConstraint();
            var xmlContent = "<test><system></system><assert></assert></test>";

            var testSuite = new TestSuite();

            try
            {
                testSuite.AssertTestCase(sut, ctr, xmlContent);
            }
            catch (AssertionException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.That(ex.Message, Is.StringContaining("empty"));
            }
            catch (Exception ex)
            {
                Assert.Fail("The exception should have been an AssertionException but was {0}.", new object[] { ex.GetType().FullName });
            }
        }

        [Test]
        public void AssertTestCase_TestCaseError_StackTraceIsFilledWithXml()
        {
            var sut = "not empty string";
            var ctrStub = new Mock<Constraint>();
            ctrStub.Setup(c => c.Matches(It.IsAny<object>())).Throws(new ExternalDependencyNotFoundException("Filename"));
            var ctr = ctrStub.Object;

            var xmlContent = "<test><system></system><assert></assert></test>";

            var testSuite = new TestSuite();

            try
            {
                testSuite.AssertTestCase(sut, ctr, xmlContent);
            }
            catch (CustomStackTraceErrorException ex)
            {
                Assert.That(ex.StackTrace, Is.EqualTo(xmlContent));
            }
            catch (Exception ex)
            {
                Assert.Fail("The exception should have been an CustomStackTraceErrorException but was {0}.", new object[] { ex.GetType().FullName });
            }
        }

        [Test]
        public void AssertTestCase_TestCaseError_MessageIsAvailable()
        {
            var sut = "not empty string";
            var ctrStub = new Mock<Constraint>();
            ctrStub.Setup(c => c.Matches(It.IsAny<object>())).Throws(new ExternalDependencyNotFoundException("Filename"));
            var ctr = ctrStub.Object;

            var xmlContent = "<test><system></system><assert></assert></test>";

            var testSuite = new TestSuite();

            try
            {
                testSuite.AssertTestCase(sut, ctr, xmlContent);
            }
            catch (CustomStackTraceErrorException ex)
            {
                Console.WriteLine(ex.Message);
                Assert.That(ex.Message, Is.StringContaining("Filename"));
            }
            catch (Exception ex)
            {
                Assert.Fail("The exception should have been a CustomStackTraceErrorException but was {0}.", new object[] { ex.GetType().FullName });
            }
        }

    }
}
