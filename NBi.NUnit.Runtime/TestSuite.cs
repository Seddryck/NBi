using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using NBi.Core;
using NBi.Xml;
using NUnit.Framework;
using NUnitCtr = NUnit.Framework.Constraints;

namespace NBi.NUnit.Runtime
{
    /// <summary>
    /// This Class is the entry point for NUnit.Framework
    /// In reality the NUnit.Framework think this class is the class containing all the fixtures. But
    /// in reality this class will just call the NBi 
    /// </summary>
    [TestFixture]
    public class TestSuite
    {
        public bool EnableAutoCategories { get; set; }

        internal XmlManager TestSuiteManager { get; private set; }
        internal TestSuiteFinder TestSuiteFinder { get; set; }
        internal ConnectionStringsFinder ConnectionStringsFinder { get; set; }
        internal ConfigurationFinder ConfigurationFinder { get; set; }

        public TestSuite()
        {
            TestSuiteManager = new XmlManager();
            TestSuiteFinder = new TestSuiteFinder();
            ConnectionStringsFinder = new ConnectionStringsFinder();
            ConfigurationFinder = new ConfigurationFinder();
        }

        internal TestSuite(XmlManager testSuiteManager, TestSuiteFinder testSuiteFinder)
        {
            TestSuiteManager = testSuiteManager;
            TestSuiteFinder = testSuiteFinder;
        }

        [Test, TestCaseSource("GetTestCases")]
        public virtual void ExecuteTestCases(TestXml test)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, string.Format("Test loaded by {0}", GetOwnFilename()));
            Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, string.Format("Test defined in {0}", TestSuiteFinder.Find()));

            //check if ignore is set to true
            if (test.Ignore)
                Assert.Ignore(test.IgnoreReason);
            else
            {
                foreach (var tc in test.Systems)
                {
                    foreach (var ctr in test.Constraints)
                    {
                        var testCase = new TestCaseFactory().Instantiate(tc, ctr);
                        AssertTestCase(testCase.SystemUnderTest, testCase.Constraint, test.Content);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the standard assertion and if needed rethrow a new AssertionException with a modified stacktrace
        /// </summary>
        /// <param name="systemUnderTest"></param>
        /// <param name="constraint"></param>
        protected internal void AssertTestCase(Object systemUnderTest, NUnitCtr.Constraint constraint, string stackTrace)
        {
            try
            {
                Assert.That(systemUnderTest, constraint);
            }
            catch (AssertionException ex)
            {
                throw new CustomStackTraceAssertionException(ex, stackTrace);
            }
            catch (NBiException ex)
            {
                throw new CustomStackTraceErrorException(ex, stackTrace);
            }
        }

        public IEnumerable<TestCaseData> GetTestCases()
        {
            TestSuiteManager.Load(TestSuiteFinder.Find());

            //Find configuration of NBi
            if (ConfigurationFinder != null)
                ApplyConfig(ConfigurationFinder.Find());

            //Find connection strings referecned from an external file
            if (ConnectionStringsFinder != null)
                TestSuiteManager.ConnectionStrings = ConnectionStringsFinder.Find();

            List<TestCaseData> testCasesNUnit = new List<TestCaseData>();

            foreach (var test in TestSuiteManager.TestSuite.Tests)
            {
                TestCaseData testCaseDataNUnit = new TestCaseData(test);
                testCaseDataNUnit.SetName(test.GetName());
                testCaseDataNUnit.SetDescription(test.Description);
                foreach (var category in test.Categories)
                {
                    testCaseDataNUnit.SetCategory(category);
                }

                //Assign auto-categories
                if (EnableAutoCategories)
                {
                    foreach (var system in test.Systems)
                        foreach (var category in system.GetAutoCategories())
                        {
                            var noSpecialCharCategory = category.Replace("-", "_");
                            testCaseDataNUnit.SetCategory(noSpecialCharCategory);
                        }
                }

                testCasesNUnit.Add(testCaseDataNUnit);
            }
            return testCasesNUnit;
        }

        public void ApplyConfig(NBiSection config)
        {
            EnableAutoCategories = config.EnableAutoCategories;
        }


        protected internal string GetOwnFilename()
        {
            //get the full location of the assembly with DaoTests in it
            var fullPath = System.Reflection.Assembly.GetAssembly(typeof(TestSuite)).Location;

            //get the filename that's in
            var fileName = Path.GetFileName( fullPath );

            return fileName;
        }

        protected internal string GetManifestName()
        {
            //get the full location of the assembly with DaoTests in it
            var fullName = System.Reflection.Assembly.GetAssembly(typeof(TestSuite)).ManifestModule.Name;

            return fullName;
        }
    }
}
