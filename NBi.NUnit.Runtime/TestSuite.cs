using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Core;
using NBi.Xml;
using NUnit.Framework;
using System.Configuration;
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
        public bool EnableAutoCategories { get { return true; } }

        internal XmlManager TestSuiteManager { get; private set; }
        internal TestSuiteFinder TestSuiteFinder { get; set; }

        public TestSuite()
        {
            TestSuiteManager = new XmlManager();
            TestSuiteFinder = new TestSuiteFinder();
        }

        internal TestSuite(XmlManager testSuiteManager, TestSuiteFinder testSuiteFinder)
        {
            TestSuiteManager = testSuiteManager;
            TestSuiteFinder = testSuiteFinder;
        }

        [Test, TestCaseSource("GetTestCases")]
        public virtual void ExecuteTestCases(TestXml test)
        {
            Console.Out.WriteLine(string.Format("Test suite loaded from {0}", GetOwnFilename()));
            Console.Out.WriteLine(string.Format("Test suite defined in {0}", TestSuiteFinder.Find()));

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
            catch (TestException ex)
            {
                throw new CustomStackTraceErrorException(ex, stackTrace);
            }
        }

        public IEnumerable<TestCaseData> GetTestCases()
        {
            TestSuiteManager.Load(TestSuiteFinder.Find());

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
                            testCaseDataNUnit.SetCategory(category);
                }

                testCasesNUnit.Add(testCaseDataNUnit);
            }
            return testCasesNUnit;
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
