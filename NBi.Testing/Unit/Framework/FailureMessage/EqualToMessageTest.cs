﻿using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Framework.FailureMessage
{
    public class EqualToMessageTest
    {

        private IEnumerable<DataRow> GetDataRows(int count)
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < count; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            return dataTable.Rows.Cast<DataRow>();
        }


        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnCorrectNumberOfRowsOnTop()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var msg = new EqualToMessage();
            msg.Build(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');

            Assert.That(lines[0], Is.EqualTo("ResultSet with 20 rows"));
        }

        [Test]
        public void RenderExpected_OneRow_ReturnCorrectNumberOfRowsOnTopWithoutPlurial()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 1; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var msg = new EqualToMessage();
            msg.Build(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');


            Assert.That(lines[0], Is.EqualTo("ResultSet with 1 row"));
        }
        
        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnSampleRowsCountAndHeaderAndSeparation()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 20; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var msg = new EqualToMessage();
            msg.Build(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');


            Assert.That(lines.Count(l => l.Contains("|")), Is.EqualTo(10 + 2));
        }

        [Test]
        public void RenderExpected_MoreThanSampleRowsCountButLessThanMaxRowsCount_ReturnEachRowAndHeaderAndSeparation()
        {
            var rowCount = 12;

            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var msg = new EqualToMessage();
            msg.Build(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');


            Assert.That(lines.Count(l => l.Contains("|")), Is.EqualTo(rowCount + 2));
        }

        [Test]
        public void RenderExpected_MoreThanMaxRowsCount_ReturnCorrectCountOfSkippedRow()
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < 22; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var msg = new EqualToMessage();
            msg.Build(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();
            var lines = value.Replace("\n", string.Empty).Split('\r');
            //Not exactly the last line but the previous due to paragraph rendering.
            var lastLine = lines.Reverse().ElementAt(1);

            Assert.That(lastLine, Is.EqualTo("12 (of 22) rows have been skipped for display purpose."));
        }

        [Test]
        [TestCase(5)]
        [TestCase(12)]
        public void RenderExpected_LessThanMaxRowsCount_DoesntDisplaySkippedRow(int rowCount)
        {
            var dataSet = new DataSet();
            var dataTable = new DataTable() { TableName = "MyTable" };
            dataTable.Columns.Add(new DataColumn("Id"));
            dataTable.Columns.Add(new DataColumn("Numeric value"));
            dataTable.Columns.Add(new DataColumn("Boolean value"));
            for (int i = 0; i < rowCount; i++)
                dataTable.LoadDataRow(new object[] { "Alpha", i, true }, false);

            var msg = new EqualToMessage();
            msg.Build(dataTable.Rows.Cast<DataRow>(), null, null);
            var value = msg.RenderExpected();

            Assert.That(value, Is.Not.StringContaining("rows have been skipped for display purpose."));
        }

        [Test]
        [TestCase(0, 5, 5, 5, 5, "Missing rows:")]
        [TestCase(5, 0, 5, 5, 5, "Unexpected rows:")]
        [TestCase(5, 5, 0, 5, 5, "Duplicated rows:")]
        [TestCase(5, 5, 5, 5, 0, "Non matching value rows:")]
        public void RenderCompared_NoSpecialRows_DoesntDisplayTextForThisKindOfRows(
            int missingRowCount
            , int unexpectedRowCount
            , int duplicatedRowCount
            , int keyMatchingRowCount
            , int nonMatchingValueRowCount
            , string unexpectedText)
        {
            var compared = ResultSetCompareResult.Build(
                    GetDataRows(missingRowCount)
                    , GetDataRows( unexpectedRowCount)
                    , GetDataRows( duplicatedRowCount)
                    , GetDataRows( keyMatchingRowCount)
                    , GetDataRows( nonMatchingValueRowCount)
                );


            var msg = new EqualToMessage();
            msg.Build(null, null, compared);
            var value = msg.RenderCompared();

            Assert.That(value, Is.Not.StringContaining(unexpectedText));
        }

        [Test]
        [TestCase(3, 0, 0, 0, 0, "Missing rows:")]
        [TestCase(0, 3, 0, 0, 0, "Unexpected rows:")]
        [TestCase(0, 0, 3, 0, 0, "Duplicated rows:")]
        [TestCase(0, 0, 0, 0, 3, "Non matching value rows:")]
        public void RenderCompared_WithSpecialRows_DisplayTextForThisKindOfRows(
            int missingRowCount
            , int unexpectedRowCount
            , int duplicatedRowCount
            , int keyMatchingRowCount
            , int nonMatchingValueRowCount
            , string expectedText)
        {
            var compared = ResultSetCompareResult.Build(
                    GetDataRows(missingRowCount)
                    , GetDataRows(unexpectedRowCount)
                    , GetDataRows(duplicatedRowCount)
                    , GetDataRows(keyMatchingRowCount)
                    , GetDataRows(nonMatchingValueRowCount)
                );


            var msg = new EqualToMessage();
            msg.Build(null, null, compared);
            var value = msg.RenderCompared();

            Assert.That(value, Is.StringContaining(expectedText));
        }
    }
}
