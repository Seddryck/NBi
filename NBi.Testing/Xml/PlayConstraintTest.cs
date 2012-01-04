using System.Collections.Generic;
using NBi.NUnit;
using NBi.Xml;
using NUnit.Framework;

namespace NBi.Testing.Xml
{
    [TestFixture]
    public class PlayConstraintTest
    {
        protected string _connectionString;

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
            _connectionString = "Data Source=.;Initial Catalog=NBi.Testing;Integrated Security=True";
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion
        
        [Test]
        public void TestCase_Play_Success()
        {
            var constraint = new QueryParserConstraint(_connectionString);
            var testCase = new TestCaseXml() { Sql = "SELECT * FROM Product;" };

            testCase.Play(constraint);
            Assert.Pass();
        }

        [Test]
        public void Test_Play_Success()
        {
            var t = new TestXml()
            {
                Constraints = new List<AbstractConstraintXml>() { new QueryParserXml() { ConnectionString = _connectionString } },
                TestCases = new List<TestCaseXml>() { new TestCaseXml() { Sql = "SELECT * FROM Product;" } }
            };

            t.Play();

            Assert.Pass();
        }
    }
}