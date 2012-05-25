#region Using directives
using NBi.Core.ResultSet;
using NUnit.Framework;
using NBiRs = NBi.Core.ResultSet;
#endregion

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class BasicResultSetComparerTest
    {
        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {
            
        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion

        
        [Test]
        public void Validate_TwoIdenticalStringsResultSets_ReturnSuccess()
        {
            var resultSet1 = new NBiRs.ResultSet("a;b;c");
            var resultSet2 = new NBiRs.ResultSet("a;b;c");

            IResultSetComparer rsc = new BasicResultSetComparer();

            //Method under test and Test conclusion
            Assert.That(rsc.Compare(resultSet1, resultSet2), Is.EqualTo(0));
        }

        [Test]
        public void Compare_TwoDifferentResultSets_ReturnDifferent()
        {
            var resultSet1 = new NBiRs.ResultSet("a;b;c");
            var resultSet2 = new NBiRs.ResultSet("a;x;c");

            IResultSetComparer rsc = new BasicResultSetComparer();

            //Method under test and Test conclusion
            Assert.That(rsc.Compare(resultSet1, resultSet2), Is.Not.EqualTo(0));
        }

    }
}
