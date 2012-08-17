using NUnit.Framework;
using NBi.NUnit.Runtime;
using NBi.Xml;

namespace NBi.Testing.Acceptance
{
    [TestFixture]
    public class RuntimeOverrider
    {
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
        

        [Test]
        [TestCase("QueryEqualToCsv.xml")]
        [TestCase("QueryEqualToQuery.xml")]
        [TestCase("QueryEqualToResultSet.xml")]
        public void RunTestSuite(string filename)
        {
            var t = new TestSuiteOverrider(filename);
            
            var tests = t.GetTestCases();
            foreach (var testCaseData in tests)
                t.ExecuteTestCases((TestXml)testCaseData.Arguments[0]);
            
        }
    }
}
