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
            
            public TestSuiteOverrider(string filename) : base()
            {
                TestSuiteFinder = new TestSuiteFinderOverrider(filename);
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

            [Ignore]
            public override void ExecuteTestCases(TestXml test)
            {
                base.ExecuteTestCases(test);
            }
        }
        
        //By Acceptance Test Suite (file) create a Test Case
        [Test]
        [TestCase("AssemblyEqualToResultSet.nbits")]
        [TestCase("QueryEqualToCsv.nbits")]
        [TestCase("QueryEqualToQuery.nbits")]
        [TestCase("QueryEqualToResultSet.nbits")]
        [TestCase("QueryEqualToResultSetWithNull.nbits")]
        [TestCase("QueryWithReference.nbits")]
        [TestCase("Ordered.nbits")]
        [TestCase("Count.nbits")]
        [TestCase("Contain.nbits")]
        [TestCase("ContainStructure.nbits")]
        [TestCase("fasterThan.nbits")]
        [TestCase("SyntacticallyCorrect.nbits")]
        [TestCase("Exists.nbits")]
        [TestCase("LinkedTo.nbits")]
        [TestCase("SubsetOfStructure.nbits")]
        [TestCase("EquivalentToStructure.nbits")]
        [TestCase("SubsetOfMembers.nbits")]
        [TestCase("EquivalentToMembers.nbits")]
        [TestCase("MatchPatternMembers.nbits")]
        [TestCase("ResultSetMatchPattern.nbits")]
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
