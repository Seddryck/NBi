#region Using directives
using System;
using System.Data;
using NBi.Core.ResultSet;
using NUnit.Framework;
using NUnit.Framework.Constraints;

#endregion

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class LineFormatterTest
    {
        [Test]
        public void GetText_ShortColumn_NoException()
        {
            // Design Dummy Column
            DataColumn col = new DataColumn("DummyColumn");
            col.ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            col.ExtendedProperties.Add("NBi::Type", ColumnType.Numeric);
            col.ExtendedProperties.Add("NBi::Tolerance", (decimal)0);

            // Design dummy table
            DataTable table = new DataTable();
            table.Columns.Add(col);

            ICellFormatter cf = LineFormatter.BuildHeader(table, 0);

            // This must not throw an exception when the header is bigger that requested size
            cf.GetText(4);
            Assert.Pass();
        }

        [Test]
        public void Build_NullAndEmptyAreDifferent_CorrectDisplay()
        {
            // Design Dummy Column
            DataColumn col = new DataColumn("DummyColumn");
            col.ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            col.ExtendedProperties.Add("NBi::Type", ColumnType.Text);

            // Design dummy table
            DataTable table = new DataTable();
            table.Columns.Add(col);

            var row = table.NewRow();
            row[0] = DBNull.Value;
            row.SetColumnError(0, "(empty)");

            ICellFormatter cf = LineFormatter.Build(row, 0);

            // This must not throw an exception when the header is bigger that requested size
            var text = cf.GetText(20);
            Assert.That(text, Is.StringContaining("(null)"));
            Assert.That(text, Is.StringContaining("<>"));
            Assert.That(text, Is.StringContaining("(empty)"));
        }

        [Test]
        public void Build_EmptyAndNullAreDifferent_CorrectDisplay()
        {
            // Design Dummy Column
            DataColumn col = new DataColumn("DummyColumn");
            col.ExtendedProperties.Add("NBi::Role", ColumnRole.Key);
            col.ExtendedProperties.Add("NBi::Type", ColumnType.Text);

            // Design dummy table
            DataTable table = new DataTable();
            table.Columns.Add(col);

            var row = table.NewRow();
            row[0] = string.Empty;
            row.SetColumnError(0, "(null)");

            ICellFormatter cf = LineFormatter.Build(row, 0);

            // This must not throw an exception when the header is bigger that requested size
            var text = cf.GetText(20);

            Assert.That(text, Is.StringContaining("(empty)"));
            Assert.That(text, Is.StringContaining("<>"));
            Assert.That(text, Is.StringContaining("(null)")); 
        }
    }
}
