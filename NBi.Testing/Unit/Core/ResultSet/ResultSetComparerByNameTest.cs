#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using NBi.Core.ResultSet;
using NUnit.Framework;
using System.Diagnostics;
#endregion

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class ResultSetComparerByNameTest
    {
        private Random random = new Random();

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
        public void Compare_SameRows_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new ResultSetComparerByName(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName"}, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_DifferentRows_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new ResultSetComparerByName(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 2 }, new object[] { "Key1", 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsByKeys_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new ResultSetComparerByName(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key1", 1 }, new object[] { "Key2", 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_SameRowsMixedColumns_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new ResultSetComparerByName(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "ValueName", "KeyName" }, new object[] { 0, "Key0" }, new object[] { 1, "Key1" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_DifferentRowsMixedColumns_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new ResultSetComparerByName(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "ValueName", "KeyName" }, new object[] { 2, "Key0" }, new object[] { 1, "Key1" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsByKeysMixedColumns_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new ResultSetComparerByName(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "ValueName", "KeyName" }, new object[] { 2, "Key2" }, new object[] { 1, "Key1" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }
        

        protected DataTable BuildDataTable(string[] columnNames, object[] firstRow, object[] secondRow)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            foreach (var columnName in columnNames)
                dt.Columns.Add(columnName);

            var dr = dt.NewRow();
            dr.ItemArray = firstRow;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr.ItemArray = secondRow;
            dt.Rows.Add(dr);

            return dt;
        }
        
        protected SettingsResultSetComparisonByName BuildSettingsKeyValue()
        {
            return BuildSettingsKeyValue(0, ColumnType.Text);
        }
        
        protected SettingsResultSetComparisonByName BuildSettingsKeyValue( decimal tolerance, ColumnType keyType)
        {
            var columnsDef = new List<IColumnDefinition>();
            columnsDef.Add(
                    new Column() { Name = "KeyName", Role = ColumnRole.Key, Type = keyType}
                    );
            columnsDef.Add(
                    new Column() { Name = "ValueName", Role = ColumnRole.Value, Type = ColumnType.Numeric, Tolerance = tolerance.ToString() }
                    );

            return new SettingsResultSetComparisonByName(
                string.Empty,
                string.Empty,
                ColumnType.Numeric,
                null,
                columnsDef
                );
        }
        
    }
}
