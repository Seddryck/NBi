﻿#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using NBi.Core.ResultSet;
using NUnit.Framework;

#endregion

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class DataRowBasedResultSetComparerTest
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
        public void Compare_SameRows_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_SameRowsNumericKeys_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.Numeric));
            var reference = BuildDataTable(new string[] { "100", "12" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "0100.00", "12.0" }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_SameRowsNumericKeysWithNumericType_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.Numeric));
            var reference = BuildDataTable(new string[] { "100", "12.750" }, new double[] { 0, 1 });
            var actual = BuildDataTableNumeric(new decimal[] { new decimal(100), new decimal(12.75) }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_SameRowsDateTimeKeys_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.DateTime));
            var reference = BuildDataTable(new string[] { "2015-01-17", "2015-01-18" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "17/01/2015", "18-01-2015" }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_SameRowsBooleanKeys_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.Boolean));
            var reference = BuildDataTable(new string[] { "yes", "no" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "True", "FALSE" }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_SameRowsDateTimeKeysWithDateTimeType_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.DateTime));
            var reference = BuildDataTable(new string[] { "2015-01-17", "2015-01-18" }, new double[] { 0, 1 });
            var actual = BuildDataTableDateTime(new DateTime[] { new DateTime(2015, 01, 17), new DateTime(2015, 01, 18) }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_SameRowsBooleanKeysWithBoolean_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.Boolean));
            var reference = BuildDataTable(new string[] { "yes", "no" }, new double[] { 0, 1 });
            var actual = BuildDataTableBoolean(new bool[] { true, false }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_DifferentRows_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key10", "Key1" }, new double[] { 10, 11 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsNumericKeysWithNumericType_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.Numeric));
            var reference = BuildDataTable(new string[] { "100", "12.750" }, new double[] { 0, 1 });
            var actual = BuildDataTableNumeric(new decimal[] { new decimal(999), new decimal(12.75) }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsNumericKeysWithDateTimeType_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.DateTime));
            var reference = BuildDataTable(new string[] { "2015-01-17", "2015-01-18" }, new double[] { 0, 1 });
            var actual = BuildDataTableDateTime(new DateTime[] { new DateTime(2015, 01, 17), new DateTime(2015, 01, 19) }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsWithHoursNumericKeysWithDateTimeType_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.DateTime));
            var reference = BuildDataTable(new string[] { "2015-01-17", "2015-01-18" }, new double[] { 0, 1 });
            var actual = BuildDataTableDateTime(new DateTime[] { new DateTime(2015, 01, 17), new DateTime(2015, 01, 18,8,0,0) }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsBooleanKeys_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.Boolean));
            var reference = BuildDataTable(new string[] { "True" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "FALSE" }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DifferentRowsBooleanKeysWithBooleanType_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(ColumnType.Boolean));
            var reference = BuildDataTable(new string[] { "True" }, new double[] { 0, 1 });
            var actual = BuildDataTableBoolean(new bool[] { false }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_UnexpectedRow_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key0", "Key1", "Key2" }, new double[] { 0, 1, 2 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_MissingRow_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key1" }, new double[] { 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DuplicatedRow_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key0", "Key1", "Key2" }, new double[] { 0, 1, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DuplicatedRowButWithDifferentValue_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key0", "Key1", "Key2" }, new double[] { 0, 1, 2 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_DuplicatedRowInRef_ThrowException()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "Key0", "Key1", "Key1" }, new double[] { 0, 1, 2 });
            var actual = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            
            //Assertion is generating an exception
            var ex = Assert.Throws<ResultSetComparerException>(delegate { comparer.Compare(reference, actual); });
            Assert.That(ex.Message, Is.StringContaining("<Key1|1>"));
            Assert.That(ex.Message, Is.StringContaining("<Key1|2>"));
        }

        [Test]
        public void Compare_SameKeysButDifferentValues_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 10, 11 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_SameKeysDifferentValuesButWithinTolerance_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue(1));
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0.5, 1.5 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_SameKeysSameValuesUselessColumnNotMatching_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValueIgnore(0));
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 }, new string[] { "Useless0", "Useless1" });
            var actual = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 }, new string[] { "0Useless0", "0Useless1" });
            

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_ObjectsVersusSameTyped_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new object[] { "Key0", "Key1" }, new object[] { "0", "1" });
            var actual = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });


            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_ObjectsVersusDifferentTyped_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new object[] { "Key0", "Key1" }, new object[] { "0", "1" });
            var actual = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 11 });


            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_ObjectsVersusSameTypedButWithPrecision_ReturnEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettingsKeyValue());
            var reference = BuildDataTable(new object[] { "Key0", "Key1" }, new object[] { "0", "1.0" });
            var actual = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });


            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }


        protected DataTable BuildDataTable(string[] keys, double[] values)
        {
            return BuildDataTable(keys, values, null);
        }

        protected DataTable BuildDataTable(object[] keys, object[] values)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("myKey");
            var valueCol = dt.Columns.Add("myValue");

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<object>(keyCol, keys[i]);
                dr.SetField<object>(valueCol, values[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected DataTable BuildDataTable(string[] keys, double[] values, string[] useless)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("myKey", typeof(string));
            var valueCol = dt.Columns.Add("myValue", typeof(double));
            var uselessCol = useless != null ? dt.Columns.Add("myUseless", typeof(string)) : null;

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<string>(keyCol, keys[i]);
                dr.SetField<double>(valueCol, values[i]);
                if (uselessCol != null)
                    dr.SetField<string>(uselessCol, useless[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected DataTable BuildDataTableNumeric(decimal[] keys, double[] values)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("myKey", typeof(decimal));
            var valueCol = dt.Columns.Add("myValue", typeof(double));

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<decimal>(keyCol, keys[i]);
                dr.SetField<double>(valueCol, values[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected DataTable BuildDataTableDateTime(DateTime[] keys, double[] values)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("myKey", typeof(DateTime));
            var valueCol = dt.Columns.Add("myValue", typeof(double));

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<DateTime>(keyCol, keys[i]);
                dr.SetField<double>(valueCol, values[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected DataTable BuildDataTableBoolean(bool[] keys, double[] values)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("myKey", typeof(bool));
            var valueCol = dt.Columns.Add("myValue", typeof(double));

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<bool>(keyCol, keys[i]);
                dr.SetField<double>(valueCol, values[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        protected ResultSetComparisonSettings BuildSettingsKeyValue()
        {
            return BuildSettingsKeyValue(0, ColumnType.Text);
        }

        protected ResultSetComparisonSettings BuildSettingsKeyValue(ColumnType keyType)
        {
            return BuildSettingsKeyValue(0, keyType);
        }

        protected ResultSetComparisonSettings BuildSettingsKeyValue(decimal tolerance)
        {
            return BuildSettingsKeyValue(tolerance, ColumnType.Text);
        }

        protected ResultSetComparisonSettings BuildSettingsKeyValue(decimal tolerance, ColumnType keyType)
        {
            var columnsDef = new List<IColumnDefinition>();
            columnsDef.Add(
                    new Column() { Index = 0, Role = ColumnRole.Key, Type = keyType}
                    );
            columnsDef.Add(
                    new Column() { Index = 1, Role = ColumnRole.Value, Type = ColumnType.Numeric, Tolerance = tolerance.ToString() }
                    );

            return new ResultSetComparisonSettings(
                ResultSetComparisonSettings.KeysChoice.First,
                ResultSetComparisonSettings.ValuesChoice.Last,
                columnsDef
                );
        }

        protected ResultSetComparisonSettings BuildSettingsKeyValueIgnore(decimal tolerance)
        {
            var columnsDef = new List<IColumnDefinition>();
            columnsDef.Add(
                    new Column() { Index = 1, Role = ColumnRole.Value, Type = ColumnType.Numeric, Tolerance = tolerance.ToString() }
                    );

            columnsDef.Add(
                    new Column() { Index = 2, Role = ColumnRole.Ignore }
                    );

            return new ResultSetComparisonSettings(
                ResultSetComparisonSettings.KeysChoice.First,
                ResultSetComparisonSettings.ValuesChoice.AllExpectFirst,
                columnsDef
                );
        }
    }
}
