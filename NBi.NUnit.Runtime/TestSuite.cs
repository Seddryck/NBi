using System.Collections.Generic;
using NBi.Xml;
using NUnit.Framework;
using System.Reflection;
using System.IO;
using System.Text;

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
        public const string DEFAULT_TESTSUITE = "TestSuite.xml";

        [Test, TestCaseSource("GetTestCases")]
        public void ExecuteTestCases(TestXml test)
        {
            test.Play();
            Assert.Pass();
        }

        public IEnumerable<TestCaseData> GetTestCases()
        {
            var mgr = new XmlManager();

            mgr.Load(GetTestSuiteConfig());

            List<TestCaseData> testCasesNUnit = new List<TestCaseData>();

            foreach (var test in mgr.TestSuite.Tests)
            {
                TestCaseData testCaseDataNUnit = new TestCaseData(test);
                testCaseDataNUnit.SetName(test.Name);
                testCaseDataNUnit.SetDescription(test.Description);
                //TODO Add Categories into XmlConfiguration
                //foreach (var category in test.Categories)
                //{
                //    testCaseDataNUnit.SetCategory(category);
                //}
                testCasesNUnit.Add(testCaseDataNUnit);
            }
            return testCasesNUnit;
        }

        public string GetTestSuiteConfig()
        {
            string configFile = Assembly.GetExecutingAssembly().Location + ".config";
            string testSuiteFile = DEFAULT_TESTSUITE;
            if (File.Exists(configFile))
            {
                using (var sr = new StreamReader(Path.GetFullPath(configFile)))
                {
                    testSuiteFile = sr.ReadToEnd();
                }
            }

            return testSuiteFile;
        }
    }
}
