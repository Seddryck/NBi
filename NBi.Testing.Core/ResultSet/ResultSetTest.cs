#region Using directives
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet;
using NUnit.Framework;
using NBiRs = NBi.Core.ResultSet;
using Moq;

#endregion

namespace NBi.Testing.Core.ResultSet
{
    [TestFixture]
    public class ResultSetTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
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
        public void Load_TwoObjectsArray_TwoRowsAndThreeColumns()
        {
            var objects = new object[] { new object[] { "A", "B", "C" }, new object[] { "D", "E", "F" } }.AsEnumerable().Cast<object[]>();

            var rs = new NBiRs.ResultSet();
            rs.Load(objects);

            Assert.That(rs.Columns.Count, Is.EqualTo(3));
            Assert.That(rs.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void Load_ThreeIRowsWithTwoICells_ThreeRowsAndTwoColumns()
        {
            var objects = new List<IRow> {
                Mock.Of<IRow> (
                    x => x.Cells == new List<ICell> {
                        Mock.Of<ICell>( y=> y.Value == "CY 2001"),
                        Mock.Of<ICell>( y=> y.Value == "1000") }
                    )
                ,
                Mock.Of<IRow>(
                    x => x.Cells == new List<ICell> {
                        Mock.Of<ICell>(y=> y.Value == "CY 2002"),
                        Mock.Of<ICell>(y => y.Value == "10.4") }
                    )
                ,
                Mock.Of<IRow>(
                   x => x.Cells == new List<ICell> {
                        Mock.Of<ICell> ( y=> y.Value == "CY 2003"),
                        Mock.Of<ICell>(y => y.Value == "200") }
                    )
            };

            var rs = new NBiRs.ResultSet();
            rs.Load(objects);

            Assert.That(rs.Columns.Count, Is.EqualTo(2));
            Assert.That(rs.Rows.Count, Is.EqualTo(3));

            Assert.That(rs.Rows[0].ItemArray[0], Is.EqualTo("CY 2001"));
            Assert.That(rs.Rows[0].ItemArray[1], Is.EqualTo("1000"));
        }

    }
}
