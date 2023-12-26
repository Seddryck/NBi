﻿#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using NBi.Core.ResultSet;
using NUnit.Framework;
using System.Diagnostics;
using NBi.Core.ResultSet.Equivalence;
#endregion

namespace NBi.Core.Testing.ResultSet.Equivalence
{
    [TestFixture]
    public class SingleRowOrdinalEquivalerTest
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
        public void Compare_SameRows_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.Numeric, null, null));
            var reference = BuildDataTable<double>(new double[] { 0, 1 });
            var actual = BuildDataTable<double>(new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.Matching));
        }

        [Test]
        public void Compare_SameRowsString_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.Text, null, null));
            var reference = BuildDataTable<string>(new string[] { "alpha", "beta" });
            var actual = BuildDataTable<string>(new string[] { "alpha", "beta" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.Matching));
        }

        [Test]
        public void Compare_SameRowsDateTime_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.DateTime, null, null));
            var reference = BuildDataTable<string>(new string[] { "2015-01-17", "2015-01-18" });
            var actual = BuildDataTable<DateTime>(new DateTime[] { new DateTime(2015, 01, 17), new DateTime(2015, 01, 18) });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.Matching));
        }

        [Test]
        public void Compare_SameRowsBoolean_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.Boolean, null, null));
            var reference = BuildDataTable<string>(new string[] { "yes", "no" });
            var actual = BuildDataTable<bool>(new bool[] { true, false });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.Matching));
        }

        [Test]
        public void Compare_DifferentRows_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.Text, null, null));
            var reference = BuildDataTable<string>(new string[] { "Value0", "Value1" });
            var actual = BuildDataTable<string>(new string[] { "ValueX", "ValueY" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsSingleNotMatching_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.Text, null, null));
            var reference = BuildDataTable<string>(new string[] { "Value0", "Value1" });
            var actual = BuildDataTable<string>(new string[] { "Value0", "ValueY" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsNumeric_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.Numeric, null, null));
            var reference = BuildDataTable<string>(new string[] { "100", "12.750" });
            var actual = BuildDataTable<decimal>(new decimal[] { new decimal(999), new decimal(12.75) });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
        }

        [Test]
        public void Compare_UnexpectedRow_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.Text, null, null));
            var reference = BuildDataEmptyTable<string>(3);
            var actual = BuildDataTable<string>(new string[] { "Value0", "Value1", "Value2" });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
            Assert.That(res.Unexpected, Has.Count.EqualTo(1));
        }

        [Test]
        public void Compare_MissingRow_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.Text, null, null));
            var reference = BuildDataTable(new string[] { "Value0", "Value1", "Value2" });
            var actual = BuildDataEmptyTable<string>(3); 

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
            Assert.That(res.Missing, Has.Count.EqualTo(1));
        }


        [Test]
        public void Compare_ObjectsVersusSameTypedButWithPrecision_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new SingleRowOrdinalEquivaler(new SettingsSingleRowOrdinalResultSet (ColumnType.Numeric, null, BuildColumnsStringDecimal()));
            var reference = BuildDataTable<string>(new string[] { "Value0", "100.50" });
            var actual = BuildDataTable<object>(new object[] { "Value0", 100.5 });


            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.Matching));
        }


        private DataTableResultSet BuildDataTable<T>(T[] values)
            => BuildDataTable<T>(values, null);

        private DataTableResultSet BuildDataTable<T>(T[] values, string[] useless)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            for (int i = 0; i < values.Length; i++)
                dt.Columns.Add("myValue" + i.ToString(), typeof(T));

            if (useless != null)
                for (int i = 0; i < useless.Length; i++)
                    dt.Columns.Add("myUseless" + i.ToString(), typeof(string));
            
            var dr = dt.NewRow();
            for (int i = 0; i < values.Length; i++)
                dr.SetField<T>(dt.Columns[i], values[i]);

            if (useless != null)
                for (int i = 0; i < useless.Length; i++)
                    dr.SetField<string>(dt.Columns[values.Length + i], useless[i]);

            dt.Rows.Add(dr);

            return new DataTableResultSet(dt);
        }

        private DataTableResultSet BuildDataEmptyTable<T>(int columnCount)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            for (int i = 0; i < columnCount; i++)
                dt.Columns.Add("myValue" + i.ToString(), typeof(T));
            
            return new DataTableResultSet(dt);
        }
        
        protected IReadOnlyCollection<IColumnDefinition> BuildColumnsStringDecimal()
        {
            var columnsDef = new List<IColumnDefinition>()
            {
                new Column() { Identifier= new ColumnOrdinalIdentifier(0), Role = ColumnRole.Value, Type = ColumnType.Text},
                new Column() { Identifier= new ColumnOrdinalIdentifier(1), Role = ColumnRole.Value, Type = ColumnType.Numeric}
            };
            return columnsDef.AsReadOnly();
        }
        
        
    }
}
