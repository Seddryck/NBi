using NUnit.Framework;
using NBi.NUnit.Runtime;
using NBi.Xml;

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
            
            private string _filename;
            public TestSuiteOverrider(string filename)
            {
                _filename = filename;
            }
            
            protected override string GetTestSuiteFileDefinition()
            {
                return @"Acceptance\Resources\" + _filename;
            }
        }
        
        //By Acceptance Test Suite (file) create a Test Case
        [Test]
        [TestCase("QueryEqualToCsv.xml")]
        [TestCase("QueryEqualToQuery.xml")]
        [TestCase("QueryEqualToResultSet.xml")]
        [TestCase("QueryEqualToResultSetWithNull.xml")]
        [TestCase("Ordered.xml")]
        [TestCase("Count.xml")]
        [TestCase("Contains.xml")]
        [TestCase("fasterThan.xml")]
        public void RunTestSuite(string filename)
        {
            var t = new TestSuiteOverrider(filename);
            
            //First retrieve the NUnit TestCases with base class (NBi.NUnit.Runtime)
            //These NUnit TestCases are defined in the Test Suite file
            var tests = t.GetTestCases();

            //Execute the NUnit TestCases one by one
            foreach (var testCaseData in tests)
                t.ExecuteTestCases((TestXml)testCaseData.Arguments[0]);
            
        }
    }
}
