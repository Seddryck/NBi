#region Using directives
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet;
using NUnit.Framework;
using NBiRs = NBi.Core.ResultSet;
using Moq;
using System.Data;
using Deedle;
using System.Diagnostics;

#endregion

namespace NBi.Core.Testing.ResultSet
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

            var rs = new DataTableResultSet();
            rs.Load(objects);

            Assert.That(rs.ColumnCount, Is.EqualTo(3));
            Assert.That(rs.Rows.Count, Is.EqualTo(2));
        }

        [Test]
        public void Load_ThreeIRowsWithTwoICells_ThreeRowsAndTwoColumns()
        {
            var objects = new List<IRow> {
                Mock.Of<IRow> (
                    x => x.Cells == new List<ICell> {
                        Mock.Of<ICell>(y => (string)(y.Value) == "CY 2001"),
                        Mock.Of<ICell>(y => (decimal)(y.Value) == 1000) }
                    )
                ,
                Mock.Of<IRow>(
                    x => x.Cells == new List<ICell> {
                        Mock.Of<ICell>(y => (string)(y.Value) == "CY 2002"),
                        Mock.Of<ICell>(y => (decimal)(y.Value) == 10.4m) }
                    )
                ,
                Mock.Of<IRow>(
                   x => x.Cells == new List<ICell> {
                        Mock.Of<ICell>(y => (string)(y.Value) == "CY 2003"),
                        Mock.Of<ICell>(y => (decimal)(y.Value) == 200) }
                    )
            };

            var rs = new DataTableResultSet();
            rs.Load(objects);

            Assert.That(rs.ColumnCount, Is.EqualTo(2));
            Assert.That(rs.Rows.Count, Is.EqualTo(3));

            Assert.That(rs[0][0], Is.EqualTo("CY 2001"));
            Assert.That(rs[0][1], Is.EqualTo(1000));
            Assert.That(rs[1][0], Is.EqualTo("CY 2002"));
            Assert.That(rs[1][1], Is.EqualTo(10.4));
        }

        [Test]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public void DataSetToDataFrame_FewRows_FastEnougth(int x)
        {
            var objects = new List<object>();
            for (int i = 0; i < x; i++)
                objects.Add(new object?[] { i, i.ToString(), null, i * 2 });


            var rs = new DataTableResultSet();
            rs.Load(objects.AsEnumerable().Cast<object[]>());
            Assert.That(rs.Rows.Count, Is.EqualTo(x));

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var frame = Frame.ReadReader(rs.CreateDataReader());
            stopWatch.Stop();
            Debug.WriteLine(stopWatch.ElapsedMilliseconds);
            Assert.That(frame.RowCount, Is.EqualTo(x));
            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThan(5000));
        }

        [Test]
        [TestCase(100)]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        public void DataFrameToDataSet_FewRows_FastEnougth(int x)
        {
            var objects = new List<object>();
            for (int i = 0; i < x; i++)
                objects.Add(new object?[] { i, i.ToString(), null, i * 2 });


            var rs = new DataTableResultSet();
            rs.Load(objects.AsEnumerable().Cast<object[]>());
            Assert.That(rs.Rows.Count, Is.EqualTo(x));
            var frame = Frame.ReadReader(rs.CreateDataReader());
            Assert.That(frame.RowCount, Is.EqualTo(x));


            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var dt = frame.ToDataTable(["yo"]);
            stopWatch.Stop();
            Assert.That(dt.Rows.Count, Is.EqualTo(x));
            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThan(5000));
        }

    }
}
