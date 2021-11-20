#region Using directives
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Moq;
using NBi.Core;
using NBi.Core.Decoration.Grouping.Commands;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Variable;
using NBi.Extensibility;
using NBi.NUnit.Builder.Helper;
using NBi.NUnit.Runtime;
using NBi.NUnit.Runtime.Configuration;
using NBi.Xml;
using NBi.Xml.Decoration;
using NBi.Xml.Decoration.Command;
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
            var filename = TestSuite.GetOwnFilename();
            Assert.That(filename, Is.EqualTo("NBi.NUnit.Runtime.dll"));
        }

        [Test]
        public void GetSelfFilename_DefaultValue_ReturnNBiNUnitRuntimedll()
        {
            var name = TestSuite.GetManifestName();
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
            Assert.That(testCase.TestName, Does.Contain("my name contains a regex").And
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
            Assert.That(testCases.ElementAt(0).TestName, Does.Contain("My test for January"));
            Assert.That(testCases.ElementAt(1).TestName, Does.Contain("My test for February"));
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
                Assert.That(ex.Message, Does.Contain("empty"));
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
                Assert.That(ex.Message, Does.Contain("Filename"));
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
                                           .GetManifestResourceStream("NBi.Testing.Unit.NUnit.Runtime.Resources.GroupXmlTestSuite.xml"))
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



        [Test]
        public void ExecuteSetup_SingleDecoration_Executed()
        {
            var commandMock = new Mock<IDecorationCommand>();
            commandMock.Setup(x => x.Execute());
            var testSuite = new TestSuite();
            testSuite.ExecuteSetup(new[] { commandMock.Object });
            commandMock.Verify(x => x.Execute(), Times.Once());
        }

        [Test]
        public void ExecuteSetup_MultipleDecorationImplicitGroup_ExecutedCorrectOrder()
        {

            var firstCommandMock = new Mock<IDecorationCommand>(MockBehavior.Strict);
            var secondCommandMock = new Mock<IDecorationCommand>(MockBehavior.Strict);
            var sequence = new MockSequence();
            firstCommandMock.InSequence(sequence).Setup(x => x.Execute());
            secondCommandMock.InSequence(sequence).Setup(x => x.Execute());

            var testSuite = new TestSuite();
            testSuite.ExecuteSetup(new[] { firstCommandMock.Object, secondCommandMock.Object });
            firstCommandMock.Verify(x => x.Execute(), Times.Once());
            secondCommandMock.Verify(x => x.Execute(), Times.Once());
        }

        [Test]
        public void ExecuteSetup_MultipleDecorationExplicitGroupSequence_ExecutedCorrectOrder()
        {
            
            var firstCommandMock = new Mock<IDecorationCommand>(MockBehavior.Strict);
            var secondCommandMock = new Mock<IDecorationCommand>(MockBehavior.Strict);
            var sequence = new MockSequence();
            firstCommandMock.InSequence(sequence).Setup(x => x.Execute());
            secondCommandMock.InSequence(sequence).Setup(x => x.Execute());

            var groupCommand = new SequentialCommand(new[] { firstCommandMock.Object, secondCommandMock.Object }, false);
            
            var testSuite = new TestSuite();
            testSuite.ExecuteSetup(new[] { groupCommand });
            firstCommandMock.Verify(x => x.Execute(), Times.Once());
            secondCommandMock.Verify(x => x.Execute(), Times.Once());
        }

        [Test]
        public void ExecuteSetup_MultipleDecorationExplicitGroupSequence_NotStartingBeforePreviousIsFinalized()
        {

            var firstCommandStub = new Mock<IDecorationCommand>(MockBehavior.Strict);
            var secondCommandStub = new Mock<IDecorationCommand>(MockBehavior.Strict);
            var sequence = new MockSequence();
            Mock<IDecorationCommand> lastReturned = null;
            firstCommandStub.InSequence(sequence).Setup(x => x.Execute()).Callback(() => { System.Threading.Thread.Sleep(100); lastReturned = firstCommandStub; });
            secondCommandStub.InSequence(sequence).Setup(x => x.Execute()).Callback(() => { System.Threading.Thread.Sleep(10); lastReturned = secondCommandStub; });

            var groupCommand = new SequentialCommand(new[] { firstCommandStub.Object, secondCommandStub.Object }, false);

            var testSuite = new TestSuite();
            testSuite.ExecuteSetup(new[] { groupCommand });
            Assert.That(lastReturned, Is.EqualTo(secondCommandStub));
        }

        [Test]
        public void ExecuteSetup_MultipleDecorationExplicitGroupSequence_BothExecuted()
        {

            var firstCommandMock = new Mock<IDecorationCommand>();
            var secondCommandMock = new Mock<IDecorationCommand>();
            firstCommandMock.Setup(x => x.Execute());
            secondCommandMock.Setup(x => x.Execute());

            var groupCommand = new ParallelCommand(new[] { firstCommandMock.Object, secondCommandMock.Object }, false);

            var testSuite = new TestSuite();
            testSuite.ExecuteSetup(new[] { groupCommand });
            firstCommandMock.Verify(x => x.Execute(), Times.Once());
            secondCommandMock.Verify(x => x.Execute(), Times.Once());
        }

        [Test]
        public void ExecuteSetup_MultipleDecorationExplicitGroupParallel_ExecutedInParallel()
        {
            var firstCommandStub = new Mock<IDecorationCommand>(MockBehavior.Strict);
            var secondCommandStub = new Mock<IDecorationCommand>(MockBehavior.Strict);
            Mock<IDecorationCommand> lastReturned = null;
            firstCommandStub.Setup(x => x.Execute()).Callback(() => { System.Threading.Thread.Sleep(100); lastReturned = firstCommandStub; });
            secondCommandStub.Setup(x => x.Execute()).Callback(() => { System.Threading.Thread.Sleep(10); lastReturned = secondCommandStub; });

            var groupCommand = new ParallelCommand(new[] { firstCommandStub.Object, secondCommandStub.Object }, false);

            var testSuite = new TestSuite();
            testSuite.ExecuteSetup(new[] { groupCommand });
            Assert.That(lastReturned, Is.EqualTo(firstCommandStub));
        }

        [Test]
        public void ExecuteSetup_MultipleDecorationRunOnce_ExecutedOnce()
        {
            var commandMock = new Mock<IDecorationCommand>();
            commandMock.Setup(x => x.Execute());

            var groupCommand = new SequentialCommand(new[] { commandMock.Object }, true);

            var testSuite = new TestSuite();
            testSuite.ExecuteSetup(new[] { groupCommand });
            commandMock.Verify(x => x.Execute(), Times.Once());
            testSuite.ExecuteSetup(new[] { groupCommand });
            commandMock.Verify(x => x.Execute(), Times.Once());
        }

        [Test]
        public void ExecuteSetup_MultipleDecorationRunMultiple_ExecutedMultiple()
        {
            var commandMock = new Mock<IDecorationCommand>();
            commandMock.Setup(x => x.Execute());

            var groupCommand = new SequentialCommand(new[] { commandMock.Object }, false);

            var testSuite = new TestSuite();
            testSuite.ExecuteSetup(new[] { groupCommand });
            commandMock.Verify(x => x.Execute(), Times.Exactly(1));
            testSuite.ExecuteSetup(new[] { groupCommand });
            commandMock.Verify(x => x.Execute(), Times.Exactly(2));
            testSuite.ExecuteSetup(new[] { groupCommand });
            commandMock.Verify(x => x.Execute(), Times.Exactly(3));
        }

        [Test]
        public void BuildSetup_SameCommandWithoutRunOnce_InstantiatedMultipleTime()
        {
            var commandXml = new CommandGroupXml();
            var firstSetupXml = new SetupXml() { Commands = new List<DecorationCommandXml>() { commandXml } };
            var secondSetupXml = new SetupXml() { Commands = new List<DecorationCommandXml>() { commandXml } };

            var testSuite = new TestSuite();
            var setupHelper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());
            var commands = new List<IDecorationCommand>();
            commands.AddRange(testSuite.BuildSetup(setupHelper, firstSetupXml));
            commands.AddRange(testSuite.BuildSetup(setupHelper, secondSetupXml));
            Assert.That(commands.Count(), Is.EqualTo(2));
            Assert.That(commands[0], Is.Not.EqualTo(commands[1]));
        }

        [Test]
        public void BuildSetup_SameCommandWithRunOnce_InstantiatedOnce()
        {
            var commandXml = new CommandGroupXml() { RunOnce=true };
            var firstSetupXml = new SetupXml() { Commands = new List<DecorationCommandXml>() { commandXml } };
            var secondSetupXml = new SetupXml() { Commands = new List<DecorationCommandXml>() { commandXml } };

            var testSuite = new TestSuite();
            var setupHelper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());
            var commands = new List<IDecorationCommand>();
            commands.AddRange(testSuite.BuildSetup(setupHelper, firstSetupXml));
            commands.AddRange(testSuite.BuildSetup(setupHelper, secondSetupXml));
            Assert.That(commands.Count(), Is.EqualTo(2));
            Assert.That(commands[0], Is.EqualTo(commands[1]));
        }

        [Test]
        public void BuildSetup_SameCommandWithChildrenWithoutRunOnce_InstantiatedEachOfThem()
        {
            var commandXml = new ConnectionWaitXml();
            var groupCommand = new CommandGroupXml() { RunOnce = false, Commands = new List<DecorationCommandXml>() { commandXml } };
            var firstSetupXml = new SetupXml() { Commands = new List<DecorationCommandXml>() { groupCommand, new ConnectionWaitXml() } };
            var secondSetupXml = new SetupXml() { Commands = new List<DecorationCommandXml>() { groupCommand, new ConnectionWaitXml() } };

            var testSuite = new TestSuite();
            var setupHelper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());
            var commands = new List<IDecorationCommand>();
            commands.AddRange(testSuite.BuildSetup(setupHelper, firstSetupXml));
            commands.AddRange(testSuite.BuildSetup(setupHelper, secondSetupXml));
            Assert.That(commands.Count(), Is.EqualTo(4));
            Assert.That(commands[0], Is.Not.EqualTo(commands[1]));
            Assert.That(commands[0], Is.Not.EqualTo(commands[2]));
            Assert.That(commands[0], Is.Not.EqualTo(commands[3]));
            Assert.That(commands[1], Is.Not.EqualTo(commands[2]));
            Assert.That(commands[1], Is.Not.EqualTo(commands[3]));
            Assert.That(commands[2], Is.Not.EqualTo(commands[3]));
        }

        [Test]
        public void BuildSetup_SameCommandWithChildrenWithRunOnce_InstantiatedOnceForThisCommand()
        {
            var commandXml = new ConnectionWaitXml();
            var groupCommand = new CommandGroupXml() { RunOnce = true, Commands = new List<DecorationCommandXml>() { commandXml } };
            var firstSetupXml = new SetupXml() { Commands = new List<DecorationCommandXml>() { groupCommand, new ConnectionWaitXml() } };
            var secondSetupXml = new SetupXml() { Commands = new List<DecorationCommandXml>() { groupCommand, new ConnectionWaitXml() } };

            var testSuite = new TestSuite();
            var setupHelper = new SetupHelper(new ServiceLocator(), new Dictionary<string, IVariable>());
            var commands = new List<IDecorationCommand>();
            commands.AddRange(testSuite.BuildSetup(setupHelper, firstSetupXml));
            commands.AddRange(testSuite.BuildSetup(setupHelper, secondSetupXml));
            Assert.That(commands.Count(), Is.EqualTo(4));
            Assert.That(commands[0], Is.Not.EqualTo(commands[1]));
            Assert.That(commands[0], Is.EqualTo(commands[2]));
            Assert.That(commands[0], Is.Not.EqualTo(commands[3]));
            Assert.That(commands[1], Is.Not.EqualTo(commands[2]));
            Assert.That(commands[1], Is.Not.EqualTo(commands[3]));
            Assert.That(commands[2], Is.Not.EqualTo(commands[3]));
        }
    }
}
