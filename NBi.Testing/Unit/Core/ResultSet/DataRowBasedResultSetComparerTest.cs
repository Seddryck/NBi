#region Using directives
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
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0,1));
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.Matching));
        }

        [Test]
        public void Compare_DifferentRows_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0, 1));
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key10", "Key1" }, new double[] { 10, 11 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_UnexpectedRow_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0, 1));
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
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0, 1));
            var reference = BuildDataTable(new string[] { "Key0", "Key1" }, new double[] { 0, 1 });
            var actual = BuildDataTable(new string[] { "Key1" }, new double[] { 1 });

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultSetCompareResult.NotMatching));
        }

        [Test]
        public void Compare_SameKeysButDifferentValues_ReturnNotEqual()
        {
            //Buiding object used during test
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0, 1));
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
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0, 1, 1));
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
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0, 1));
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
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0, 1));
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
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0, 1));
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
            var comparer = new DataRowBasedResultSetComparer(BuildSettings(0, 1));
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

        protected ResultSetComparaisonSettings BuildSettings(int key, int value)
        {
            return BuildSettings(key, value, 0);
        }

        protected ResultSetComparaisonSettings BuildSettings(int key, int value, decimal tolerance)
        {
            var settings = new ResultSetComparaisonSettings(
                new List<int>() { key },
                new List<int>() { value },
                new List<decimal>() { tolerance }
            );

            return settings;
        }
    }
}
