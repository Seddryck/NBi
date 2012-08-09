#region Using directives
using System.Collections.Generic;
using System.Linq;
using NBi.Xml.Constraints.EqualTo;
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
            var objects = new RowXml[] { 
                new RowXml {
                    _cells= new List<CellXml> {
                        new CellXml() {Value= "CY 2001"},
                        new CellXml() {Value= "1000"}
                    }
                }, 
                new RowXml {
                     _cells= new List<CellXml> {
                        new CellXml() {Value= "CY 2002"},
                        new CellXml() {Value= "10.4"}
                    }
                }, 
                new RowXml {
                     _cells= new List<CellXml> {
                        new CellXml() {Value= "CY 2003"},
                        new CellXml() {Value= "200"}
                    }
                }  
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
