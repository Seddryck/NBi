using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            Console.Out.WriteLine("Loading TestSuite");
            Console.Out.WriteLine("Test suite defined in " + GetTestSuiteFileDefinition());
            foreach (var tc in test.Systems)
            {
                foreach (var ctr in test.Constraints)
                {
                    var nUnitCtr = ConstraintFactory.Instantiate(ctr, tc.GetType());
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

        protected virtual string GetTestSuiteFileDefinition()
        {
            string assem = Path.GetFullPath((new System.Uri(Assembly.GetExecutingAssembly().CodeBase)).AbsolutePath).Replace("%20"," ");
            string configFile = Path.Combine(Path.GetDirectoryName(assem), Path.GetFileNameWithoutExtension(assem) + ".config");
            
            //Set the default TestSuite
            string testSuiteFile = DEFAULT_TESTSUITE;
            
            //Try to find a config file, if existing take the path inside for the TestSuite
            Console.Out.WriteLine("Looking after config file located at '{0}'", configFile);
            if (File.Exists(configFile))
            {
                Console.Out.WriteLine("Config File found!");
                using (var sr = new StreamReader(configFile))
                {
                    testSuiteFile = sr.ReadToEnd();
                }
            }
            else
            {
                // If no config file is registered then search the first "nbits" (NBi Test Suite) file
                Console.Out.WriteLine("No config file found.");
                Console.Out.WriteLine("Looking after 'nbits' files ...");
                var files = System.IO.Directory.GetFiles(Path.GetDirectoryName(assem), "*.nbits");
                if (files.Count() == 1)
                {
                    Console.Out.WriteLine("'{0}' found, using it!", files[0]);
                    testSuiteFile = files[0];
                }
                else if (files.Count() > 1)
                {
                    Console.Out.WriteLine("{0} 'nbits' files found, using the first found: '{1}'!", files.Count(), files[0]);
                    testSuiteFile = files[0];
                }
                else
                    Console.Out.WriteLine("No 'nbits' file found");
            }

            return testSuiteFile;
        }
    }
}
