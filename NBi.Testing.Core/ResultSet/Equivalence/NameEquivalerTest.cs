﻿#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using NBi.Core.ResultSet;
using NUnit.Framework;
using System.Diagnostics;
using NBi.Core.ResultSet.Analyzer;
using NBi.Core.ResultSet.Equivalence;
#endregion

namespace NBi.Core.Testing.ResultSet.Equivalence
{
    [TestFixture]
    public class NameComparerTest
    {
        private readonly Random random = new Random();

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
        public void Compare_SameRows_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new NameEquivaler(AnalyzersFactory.EqualTo(), BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.Matching));
        }

        [Test]
        public void Compare_DifferentRows_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new NameEquivaler(AnalyzersFactory.EqualTo(), BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 2 }, new object[] { "Key1", 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsByKeys_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new NameEquivaler(AnalyzersFactory.EqualTo(), BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key1", 1 }, new object[] { "Key2", 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
        }

        [Test]
        public void Compare_SameRowsMixedColumns_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new NameEquivaler(AnalyzersFactory.EqualTo(), BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "ValueName", "KeyName" }, new object[] { 0, "Key0" }, new object[] { 1, "Key1" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.Matching));
        }

        [Test]
        public void Compare_DifferentRowsMixedColumns_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new NameEquivaler(AnalyzersFactory.EqualTo(), BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "ValueName", "KeyName" }, new object[] { 2, "Key0" }, new object[] { 1, "Key1" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsByKeysMixedColumns_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new NameEquivaler(AnalyzersFactory.EqualTo(), BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "KeyName", "ValueName" }, new object[] { "Key0", 0 }, new object[] { "Key1", 1 });
            var actual = BuildDataTable(new string[] { "ValueName", "KeyName" }, new object[] { 2, "Key2" }, new object[] { 1, "Key1" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
        }


        private DataTableResultSet BuildDataTable(string[] columnNames, object[] firstRow, object[] secondRow)
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

            return new DataTableResultSet(dt);
        }

        protected SettingsNameResultSet BuildSettingsKeyValue()
        {
            return BuildSettingsKeyValue(0, ColumnType.Text);
        }

        protected SettingsNameResultSet BuildSettingsKeyValue(decimal tolerance, ColumnType keyType)
        {
            var columnsDef = new List<IColumnDefinition>() {
                    new Column() { Identifier = new ColumnNameIdentifier("KeyName"), Role = ColumnRole.Key, Type = keyType},
                    new Column() { Identifier = new ColumnNameIdentifier("ValueName"), Role = ColumnRole.Value, Type = ColumnType.Numeric, Tolerance = tolerance.ToString() }
            };

            return new SettingsNameResultSet(
                string.Empty,
                string.Empty,
                ColumnType.Numeric,
                null,
                columnsDef
                );
        }

    }
}
