using System.IO;
using NBi.NUnit.Runtime;
using NBi.Xml;
using NUnit.Framework;
using System.Diagnostics;
using System.Reflection;
using NBi.Framework;
using NBi.Core;
using System.Collections.Generic;
using NBi.Core.Configuration;

namespace NBi.Testing.Acceptance
{
    
    public abstract class BaseRuntimeOverrider
    {
        public virtual void RunPositiveTestSuite(string filename)
        {
            var ignoredTests = new List<string>();
            var testSuite = new TestSuiteOverrider(@"Positive\" + filename);

            //First retrieve the NUnit TestCases with base class (NBi.NUnit.Runtime)
            //These NUnit TestCases are defined in the Test Suite file
            var tests = testSuite.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
            {
                try
                {
                    testSuite.ExecuteTestCases((TestXml)testCaseData.Arguments[0]);
                }
                catch (IgnoreException)
                {
                    Trace.WriteLineIf(NBiTraceSwitch.TraceWarning, $"Not stopping the test suite, continue on ignore.");
                    ignoredTests.Add(((TestXml)testCaseData.Arguments[0]).Name);
                }
            }

            if (ignoredTests.Count>0)
                Assert.Inconclusive($"At least one test has been skipped. Check if it was expected. List of ignored tests: '{string.Join("', '", ignoredTests)}'");
        }

        public virtual void RunPositiveTestSuiteWithConfig(string filename)
        {
            var testSuite = new TestSuiteOverrider(@"Positive\" + filename, @"Positive\" + filename);

            //First retrieve the NUnit TestCases with base class (NBi.NUnit.Runtime)
            //These NUnit TestCases are defined in the Test Suite file
            var tests = testSuite.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
                testSuite.ExecuteTestCases((TestXml)testCaseData.Arguments[0], testSuite.Configuration);
        }

        public virtual void RunNegativeTestSuite(string filename)
        {
            var testSuite = new TestSuiteOverrider(@"Negative\" + filename);
            //These NUnit TestCases are defined in the Test Suite file
            var tests = testSuite.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
            {
                var testXml = (TestXml)testCaseData.Arguments[0];
                try
                {
                    testSuite.ExecuteTestCases(testXml);
                    Assert.Fail("The test named '{0}' (uid={1}) and defined in '{2}' should have failed but it hasn't."
                        , testXml.Name
                        , testXml.UniqueIdentifier
                        , filename);
                }
                catch (CustomStackTraceAssertionException ex)
                {
                    using (Stream stream = Assembly.GetCallingAssembly()
                                           .GetManifestResourceStream(
                                                "NBi.Testing.Acceptance.Resources.Negative."
                                                + filename.Replace(".nbits", string.Empty)
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

        public virtual void RunNegativeTestSuiteWithConfig(string filename)
        {
            var testSuite = new TestSuiteOverrider(@"Negative\" + filename, @"Negative\" + filename);

            //First retrieve the NUnit TestCases with base class (NBi.NUnit.Runtime)
            //These NUnit TestCases are defined in the Test Suite file
            var tests = testSuite.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
            {
                var testXml = (TestXml)testCaseData.Arguments[0];
                try
                {
                    testSuite.ExecuteTestCases(testXml);
                    Assert.Fail("The test named '{0}' (uid={1}) and defined in '{2}' should have failed but it hasn't."
                        , testXml.Name
                        , testXml.UniqueIdentifier
                        , filename);
                }
                catch (CustomStackTraceAssertionException ex)
                {
                    using (Stream stream = Assembly.GetCallingAssembly()
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

        public virtual void RunIgnoredTests(string filename)
        {
            var testSuite = new TestSuiteOverrider(@"Ignored\" + filename);

            //First retrieve the NUnit TestCases with base class (NBi.NUnit.Runtime)
            //These NUnit TestCases are defined in the Test Suite file
            var tests = testSuite.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
            {
                var isSuccess = false;
                try
                {
                    testSuite.ExecuteTestCases((TestXml)testCaseData.Arguments[0]);
                }
                catch (IgnoreException)
                {
                    isSuccess = true;
                    Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, $"Expectation was met: test ignored.");
                }
                Assert.That(isSuccess);
            }
        }
    }
}
