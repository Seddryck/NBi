#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Moq;
using NBi.Core;
using NBi.Core.ResultSet;
using NBi.NUnit.Runtime;
using NBi.NUnit.Runtime.Configuration;
using NBi.Xml;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NBi.Xml.Variables;
using NBi.Xml.Variables.Sequence;
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
            var testSuite = new TestSuite();
            var filename = testSuite.GetOwnFilename();
            Assert.That(filename, Is.EqualTo("NBi.NUnit.Runtime.dll"));
        }

        [Test]
        public void GetSelfFilename_DefaultValue_ReturnNBiNUnitRuntimedll()
        {
            var testSuite = new TestSuite();
            var name = testSuite.GetManifestName();
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
                        Caption = "My Caption",
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
            var testSuiteFinderStub = new Mock<TestSuiteProvider>();
            testSuiteFinderStub.Setup(finder => finder.GetFilename(string.Empty)).Returns(string.Empty);

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
        public void GetTestCases_TestWithInstanceSettling_TestNameIsCorrect()
        {
            var testSuiteXml = new TestSuiteXml()
            {
                Tests = new List<TestXml>()
                {
                    new TestXml()
                    {
                        Name = "~My test for {@month:MMMM}",
                        InstanceSettling = new InstanceSettlingXml()
                        {
                            Variable = new InstanceVariableXml()
                            {
                                Name = "month",
                                Type = ColumnType.DateTime,
                                SentinelLoop = new SentinelLoopXml()
                                    { Seed = "2019-01-01", Terminal = "2019-02-01", Step = "1 month" }
                            }
                        }
                    }
                }
            };

            //Building a stub for TestSuiteManager
            var testSuiteManagerStub = new Mock<XmlManager>();
            testSuiteManagerStub.Setup(mgr => mgr.Load(It.IsAny<string>()));
            testSuiteManagerStub.Setup(mgr => mgr.TestSuite).Returns(testSuiteXml);

            //Building a stub for TestSuiteFinder
            var testSuiteFinderStub = new Mock<TestSuiteProvider>();
            testSuiteFinderStub.Setup(finder => finder.GetFilename(string.Empty)).Returns(string.Empty);

            var testSuite = new TestSuite(testSuiteManagerStub.Object, testSuiteFinderStub.Object);

            //Call the method to test
            var testCases = testSuite.GetTestCases();

            //Assertion
            Assert.That(testCases.ElementAt(0).TestName, Is.StringContaining("My test for January"));
            Assert.That(testCases.ElementAt(1).TestName, Is.StringContaining("My test for February"));
        }

        [Test]
        public void GetTestCases_TestWithInstanceSettlingCategories_CorrectCategories()
        {
            var testSuiteXml = new TestSuiteXml()
            {
                Tests = new List<TestXml>()
                {
                    new TestXml()
                    {
                        Name = "Youpla",
                        InstanceSettling = new InstanceSettlingXml()
                        {
                            Variable = new InstanceVariableXml()
                            {
                                Name = "month",
                                Type = ColumnType.DateTime,
                                SentinelLoop = new SentinelLoopXml()
                                    { Seed = "2019-01-01", Terminal = "2019-02-01", Step = "1 month" }
                            },
                            Categories = new List<string>() { "~{@month:MMMM}", "~{@month:yy}" },
                        },
                        Categories = new List<string>() { "Category" },
                    }
                }
            };

            //Building a stub for TestSuiteManager
            var testSuiteManagerStub = new Mock<XmlManager>();
            testSuiteManagerStub.Setup(mgr => mgr.Load(It.IsAny<string>()));
            testSuiteManagerStub.Setup(mgr => mgr.TestSuite).Returns(testSuiteXml);

            //Building a stub for TestSuiteFinder
            var testSuiteFinderStub = new Mock<TestSuiteProvider>();
            testSuiteFinderStub.Setup(finder => finder.GetFilename(string.Empty)).Returns(string.Empty);

            var testSuite = new TestSuite(testSuiteManagerStub.Object, testSuiteFinderStub.Object);

            //Call the method to test
            var testCases = testSuite.GetTestCases();

            //Assertion
            Assert.That(testCases.ElementAt(0).Categories, Has.Member("January"));
            Assert.That(testCases.ElementAt(0).Categories, Has.Member("19"));
            Assert.That(testCases.ElementAt(0).Categories, Has.Member("Category"));
            Assert.That(testCases.ElementAt(1).Categories, Has.Member("February"));
            Assert.That(testCases.ElementAt(1).Categories, Has.Member("19"));
            Assert.That(testCases.ElementAt(1).Categories, Has.Member("Category"));
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
                Assert.Fail("The exception should have been an AssertionException but was {0}.\r\n{1}", new object[] { ex.GetType().FullName, ex.StackTrace });
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
                Assert.Fail("The exception should have been an AssertionException but was {0}.\r\n{1}", new object[] { ex.GetType().FullName, ex.StackTrace });
            }
        }

        [Test]
        [Ignore]
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
                if (ex.InnerException == null)
                    Assert.Fail("The exception should have been an CustomStackTraceErrorException but was {0}.\r\n{1}", new object[] { ex.GetType().FullName, ex.StackTrace });
                else
                    Assert.Fail("The exception should have been an CustomStackTraceErrorException but was something else. The inner exception is {0}.\r\n{1}", new object[] { ex.InnerException.GetType().FullName, ex.InnerException.StackTrace });
            }
        }

        [Test]
        [Ignore]
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
                //Console.WriteLine(ex.Message);
                Assert.That(ex.Message, Is.StringContaining("Filename"));
            }
            catch (Exception ex)
            {
                if (ex.InnerException == null)
                    Assert.Fail("The exception should have been an CustomStackTraceErrorException but was {0}.\r\n{1}", new object[] { ex.GetType().FullName, ex.StackTrace });
                else
                    Assert.Fail("The exception should have been an CustomStackTraceErrorException but was something else. The inner exception is {0}.\r\n{1}", new object[] { ex.InnerException.GetType().FullName, ex.InnerException.StackTrace });
            }
        }

        [Test]
        public void BuildTestCases_WithGroups_AllTestsLoaded()
        {
            // Declare an object variable of the type to be deserialized.
            var manager = new XmlManager();

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.GroupXmlTestSuite.xml"))
            using (StreamReader reader = new StreamReader(stream))
            {
                manager.Read(reader);
            }

            var testSuite = new TestSuite(manager);
            var testCases = testSuite.BuildTestCases();
            Assert.That(testCases.Count(), Is.EqualTo(2 + 2 + 1 + 1));
        }

        [Test]
        public void ApplyConfig_OneOverridenVariable_Set()
        {
            var testSuite = new TestSuite(new XmlManager());
            var config = new NBiSection();
            config.Variables.Add(new VariableElement("myVar", "123.12", ColumnType.Numeric));

            testSuite.ApplyConfig(config);
            Assert.That(TestSuite.OverridenVariables.Count, Is.EqualTo(1));
            Assert.That(TestSuite.OverridenVariables.ContainsKey("myVar"), Is.True);
            Assert.That(TestSuite.OverridenVariables["myVar"], Is.EqualTo(123.12));
        }

    }
}
