using System.IO;
using NBi.NUnit.Runtime;
using NBi.Xml;
using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;
using NBi.Framework;
using System.Configuration;

namespace NBi.Testing.Acceptance
{
    /// <summary>
    /// This class is the only one in the namespace "NBi.Testing.Acceptance" with a TestFixture.
    /// NUnit (more specifically the SimpleTestRunner created in method Run of class TestSuiteRunner) 
    /// will load this class as the entry point for Acceptance Test Suites.
    /// </summary>
    [TestFixture]
    public class RuntimeOverrider
    {
        
        //This class overrides the search for TestSuiteDefinitionFile
        //The filename is given by the TestCase here under
        public class TestSuiteOverrider : TestSuite
        {
            public TestSuiteOverrider(string filename)
                : this(filename, null)
            {
            }

            public TestSuiteOverrider(string filename, string configFilename) : base()
            {
                TestSuiteFinder = new TestSuiteFinderOverrider(filename);
                ConfigurationFinder = new ConfigurationFinderOverrider(configFilename);
            }
            
            internal class TestSuiteFinderOverrider : TestSuiteFinder
            {
                private readonly string filename;
                public TestSuiteFinderOverrider(string filename)
                {
                    this.filename = filename;
                }
                
                protected internal override string Find()
                {
                    return @"Acceptance\Resources\" + filename;
                }
            }

            internal class ConfigurationFinderOverrider : ConfigurationFinder
            {
                private readonly string filename;
                public ConfigurationFinderOverrider(string filename)
                {
                    this.filename = filename;
                }
                protected internal override NBiSection Find()
                {
                    if (!string.IsNullOrEmpty(filename))
                    {
                        var configuration = ConfigurationManager.OpenExeConfiguration(@"Acceptance\Resources\" + filename);

                        var section = (NBiSection)(configuration.GetSection("nbi"));
                        if (section != null)
                            return section;
                    }
                    return new NBiSection();
                }
            }

            [Ignore]
            public override void ExecuteTestCases(TestXml test)
            {
                base.ExecuteTestCases(test);
            }

            [Ignore]
            public void ExecuteTestCases(TestXml test, ITestConfiguration configuration)
            {
                base.Configuration = configuration;
                base.ExecuteTestCases(test);
            }
        }

        [TestFixtureSetUp]
        public void SetupMethods()
        {
            //Build the fullpath for the file to read
            Directory.CreateDirectory("Etl");
            DiskOnFile.CreatePhysicalFile(@"Etl\Sample.dtsx", "NBi.Testing.Integration.SqlServer.IntegrationService.Resources.Sample.dtsx");
        }
        
        //By Acceptance Test Suite (file) create a Test Case
        [Test]
        //[TestCase("AssemblyEqualToResultSet.nbits")]
        //[TestCase("QueryEqualToWithParameter.nbits")]
        //[TestCase("QueryEqualToCsv.nbits")]
        [TestCase("QueryEqualToCsvWithProfile.nbits")]
        //[TestCase("QueryEqualToQuery.nbits")]
        //[TestCase("QueryEqualToResultSet.nbits")]
        //[TestCase("QueryEqualToResultSetWithNull.nbits")]
        //[TestCase("QueryWithReference.nbits")]
        //[TestCase("Ordered.nbits")]
        //[TestCase("Count.nbits")]
        //[TestCase("Contain.nbits")]
        //[TestCase("ContainStructure.nbits")]
        //[TestCase("fasterThan.nbits")]
        //[TestCase("SyntacticallyCorrect.nbits")]
        //[TestCase("Exists.nbits")]
        //[TestCase("LinkedTo.nbits")]
        //[TestCase("SubsetOfStructure.nbits")]
        //[TestCase("EquivalentToStructure.nbits")]
        //[TestCase("SubsetOfMembers.nbits")]
        //[TestCase("EquivalentToMembers.nbits")]
        //[TestCase("MatchPatternMembers.nbits")]
        //[TestCase("ResultSetMatchPattern.nbits")]
        //[TestCase("QueryWithParameters.nbits")]
        //[TestCase("EvaluateRows.nbits")]
        //[TestCase("ReportEqualTo.nbits")]
        //[TestCase("Etl.nbits")]
        //[TestCase("Decoration.nbits")]
        //[TestCase("QueryRowCount.nbits")]
        //[TestCase("Is.nbits")]
        //[TestCase("QueryEqualToXml.nbits")]
        //[Category("Acceptance")]
        public void RunPositiveTestSuite(string filename)
        {
            var t = new TestSuiteOverrider(@"Positive\" + filename);
            
            //First retrieve the NUnit TestCases with base class (NBi.NUnit.Runtime)
            //These NUnit TestCases are defined in the Test Suite file
            var tests = t.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
                t.ExecuteTestCases((TestXml)testCaseData.Arguments[0]);
            
        }

        [Test]
        [TestCase("QueryEqualToResultSetProvider.nbits")]
        [Category("Acceptance")]
        public void RunPositiveTestSuiteWithConfig(string filename)
        {
            var t = new TestSuiteOverrider(@"Positive\" + filename, @"Positive\" + filename);

            //First retrieve the NUnit TestCases with base class (NBi.NUnit.Runtime)
            //These NUnit TestCases are defined in the Test Suite file
            var tests = t.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
                t.ExecuteTestCases((TestXml)testCaseData.Arguments[0]);
        }

        [Test]
        [TestCase("DataRowsMessage.nbits")]
        [TestCase("ItemsMessage.nbits")]
        [Category("Acceptance")]
        public void RunNegativeTestSuite(string filename)
        {
            var t = new TestSuiteOverrider(@"Negative\" + filename);
            //These NUnit TestCases are defined in the Test Suite file
            var tests = t.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
            {
                var testXml = (TestXml)testCaseData.Arguments[0];
                try
                {
                    t.ExecuteTestCases(testXml);
                    Assert.Fail("The test named '{0}' (uid={1}) and defined in '{2}' should have failed but it hasn't."
                        , testXml.Name
                        , testXml.UniqueIdentifier
                        , filename);
                }
                catch (CustomStackTraceAssertionException ex)
                {
                    using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(
                                                "NBi.Testing.Acceptance.Resources.Negative." 
                                                + filename.Replace(".nbits",string.Empty) 
                                                + "-" + testXml.UniqueIdentifier + ".txt"))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            //Debug.WriteLine(ex.Message);
                            Assert.That(ex.Message, Is.EqualTo(reader.ReadToEnd()));
                        }
                        Assert.That(ex.StackTrace, Is.Not.Null.Or.Empty);
                        Assert.That(ex.StackTrace, Is.EqualTo(testXml.Content));
                    }
                }
            }
        }

        [Test]
        [TestCase("Config-Full.nbits")]
        [TestCase("Config-Light.nbits")]
        [Category("Acceptance")]
        public void RunNegativeTestSuiteWithConfig(string filename)
        {
            var t = new TestSuiteOverrider(@"Negative\" + filename, @"Negative\" + filename);

            //First retrieve the NUnit TestCases with base class (NBi.NUnit.Runtime)
            //These NUnit TestCases are defined in the Test Suite file
            var tests = t.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
            {
                var testXml = (TestXml)testCaseData.Arguments[0];
                try
                {
                    t.ExecuteTestCases(testXml);
                    Assert.Fail("The test named '{0}' (uid={1}) and defined in '{2}' should have failed but it hasn't."
                        , testXml.Name
                        , testXml.UniqueIdentifier
                        , filename);
                }
                catch (CustomStackTraceAssertionException ex)
                {
                    using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream(
                                                "NBi.Testing.Acceptance.Resources.Negative."
                                                + filename.Replace(".nbits", string.Empty)
                                                + "-" + testXml.UniqueIdentifier + ".txt"))
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            var expected = reader.ReadToEnd();
                            //Debug.WriteLine(expected);
                            //Debug.WriteLine("");
                            Debug.WriteLine(ex.Message);
                            Assert.That(ex.Message, Is.EqualTo(expected));
                        }
                        Assert.That(ex.StackTrace, Is.Not.Null.Or.Empty);
                        Assert.That(ex.StackTrace, Is.EqualTo(testXml.Content));
                    }
                }
            }
        }
    }
}
