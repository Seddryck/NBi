#region Using directives
using System.Linq;
using NUnit.Framework;
using NBiRs = NBi.Core.ResultSet;

#endregion

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class ResultSetTest
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
        public void ctor_TwoObjectsArray_TwoRowsAndTwoColumns()
        {
            var objects = new object[] { new object[] { "A", "B", "C" }, new object[] { "D", "E", "F" } }.AsEnumerable().Cast<object[]>();

            var rs = new NBiRs.ResultSet();
            rs.Load(objects);

            Assert.That(rs.Columns.Count, Is.EqualTo(3));
            Assert.That(rs.Rows.Count, Is.EqualTo(2));
        }

    }
}
