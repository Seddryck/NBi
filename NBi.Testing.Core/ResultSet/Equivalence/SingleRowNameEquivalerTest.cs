#region Using directives
using System;
using System.Collections.Generic;
using System.Data;
using NBi.Core.ResultSet;
using NUnit.Framework;
using System.Diagnostics;
using NBi.Core.ResultSet.Equivalence;
using Moq;
using NBi.Core.Scalar.Comparer;
#endregion

namespace NBi.Core.Testing.ResultSet.Equivalence
{
    [TestFixture]
    public class SingleRowNameEquivalerTest
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
        public void Compare_NonSimilarRow_ReturnEqual()
        {
            var myColumnDefinition = Mock.Of<IColumnDefinition>(
                    x => x.Type == ColumnType.Numeric
                    && x.Role == ColumnRole.Value
                    && x.Identifier == new ColumnNameIdentifier("A")
                    && x.Tolerance == "0.1"
                    && x.IsToleranceSpecified == true
                );

            var comparer = new SingleRowNameEquivaler(new SettingsSingleRowNameResultSet(ColumnType.Numeric, NumericAbsoluteTolerance.None, [myColumnDefinition]));
            var reference = BuildDataTable<double>([0, 1]);
            reference.GetColumn(0)?.Rename("A");
            var actual = BuildDataTable<double>([2, 1]);
            actual.GetColumn(0)?.Rename("A");

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.NotMatching));
        }

        [Test]
        public void Compare_SimilarRow_ReturnEqual()
        {
            var myColumnDefinition = Mock.Of<IColumnDefinition>(
                    x => x.Type == ColumnType.Numeric
                    && x.Role == ColumnRole.Value
                    && x.Identifier == new ColumnNameIdentifier("A")
                    && x.Tolerance == "0.1"
                    && x.IsToleranceSpecified == true
                );

            var comparer = new SingleRowNameEquivaler(new SettingsSingleRowNameResultSet(ColumnType.Numeric, NumericAbsoluteTolerance.None, [myColumnDefinition]));
            var reference = BuildDataTable(new double[] { 0, 1 });
            reference.GetColumn(0)?.Rename("A");
            var actual = BuildDataTable(new double[] { 0.05, 1 });
            actual.GetColumn(0)?.Rename("A");

            //Call the method to test
            var res = comparer.Compare(reference, actual);

            //Assertion
            Assert.That(res, Is.EqualTo(ResultResultSet.Matching));
        }

        private DataTableResultSet BuildDataTable<T>(T[] values)
            => BuildDataTable<T>(values, []);

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
                    dr.SetField(dt.Columns[values.Length + i], useless[i]);

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
                new Column(new ColumnOrdinalIdentifier(0), ColumnRole.Value, ColumnType.Text),
                new Column(new ColumnOrdinalIdentifier(1), ColumnRole.Value, ColumnType.Numeric)
            };
            return columnsDef.AsReadOnly();
        }
    }
}
