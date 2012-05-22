using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NBi.Xml;
using NUnit.Framework;
using NBi.NUnit;
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
        public const string DEFAULT_TESTSUITE = "TestSuite.xml";

        [Test, TestCaseSource("GetTestCases")]
        public void ExecuteTestCases(TestXml test)
        {
            Console.Out.WriteLine("Test suite defined in " + GetTestSuiteFileDefinition());
            foreach (var tc in test.Systems)
            {
                foreach (var ctr in test.Constraints)
                {
                    var nUnitCtr = ConstraintFactory.Instantiate(ctr);
                    Assert.That(tc.Instantiate(), nUnitCtr);
                }
            }
        }

        public IEnumerable<TestCaseData> GetTestCases()
        {
            var mgr = new XmlManager();

            mgr.Load(GetTestSuiteFileDefinition());

            List<TestCaseData> testCasesNUnit = new List<TestCaseData>();

            foreach (var test in mgr.TestSuite.Tests)
            {
                TestCaseData testCaseDataNUnit = new TestCaseData(test);
                testCaseDataNUnit.SetName(test.Name);
                testCaseDataNUnit.SetDescription(test.Description);
                foreach (var category in test.Categories)
                {
                    testCaseDataNUnit.SetCategory(category);
                }
                testCasesNUnit.Add(testCaseDataNUnit);
            }
            return testCasesNUnit;
        }

        protected string GetTestSuiteFileDefinition()
        {
            string assem = Path.GetFullPath((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath).Replace("%20"," ");
            string configFile = Path.Combine(Path.GetDirectoryName(assem), Path.GetFileNameWithoutExtension(assem) + ".config");
            Console.Out.WriteLine(configFile);
            string testSuiteFile = DEFAULT_TESTSUITE;
            if (File.Exists(configFile))
            {
                Console.Out.WriteLine("Existing Config File");
                using (var sr = new StreamReader(configFile))
                {
                    testSuiteFile = sr.ReadToEnd();
                }
            }
            else 
                Console.Out.WriteLine("Non Existing Config File");

            return testSuiteFile;
        }
    }
}
