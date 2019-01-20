using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage.Markdown.Helper;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Core.ResultSet.Lookup;

namespace NBi.Testing.Unit.Framework.FailureMessage.Markdown.Helper
{
    public class LookupTableHelperTest
    {
        [Test]
        public void Render_OneViolationWithOneRecordOfOneField_Correct()
        {
            var candidateTable = new DataTable() { TableName = "MyTable" };
            candidateTable.Columns.Add(new DataColumn("ForeignKey"));
            candidateTable.Columns.Add(new DataColumn("Numeric value"));
            candidateTable.Columns.Add(new DataColumn("Boolean value"));
            candidateTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            candidateTable.LoadDataRow(new object[] { "Beta", 20, false }, false);

            var foreignKeyDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value };

            var keyMappings = new ColumnMappingCollection() { new ColumnMapping(foreignKeyDefinition.Identifier, ColumnType.Text) };
            var valueMappings = new ColumnMappingCollection() { new ColumnMapping(numericDefinition.Identifier, ColumnType.Numeric) };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { candidateTable.Columns[1] , new LookupMatchesViolationData(false, 15) },
                },
            };
            var association = new LookupMatchesViolationComposite(candidateTable.Rows[0], records);

            var msg = new LookupTableHelper(new[] { association }, new[] { foreignKeyDefinition, numericDefinition });
            var value = msg.Render().ToMarkdown();

            Console.WriteLine(value);

            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(4));

            var secondLineIndex = value.IndexOf('\n');
            var dashLineIndex = value.IndexOf('\n', secondLineIndex + 1);
            var firstLineViolationIndex = value.IndexOf('\n', dashLineIndex + 1);
            var dashLine = value.Substring(dashLineIndex + 1, firstLineViolationIndex - dashLineIndex - 2);

            Assert.That(dashLine.Distinct().Count(), Is.EqualTo(3));
            Assert.That(dashLine.Distinct(), Has.Member(' '));
            Assert.That(dashLine.Distinct(), Has.Member('-'));
            Assert.That(dashLine.Distinct(), Has.Member('|'));

            var firstLineViolation = value.Substring(firstLineViolationIndex + 1, value.Length - firstLineViolationIndex - 1);
            Assert.That(firstLineViolation, Is.StringContaining("| 10 <> 15"));
        }

        [Test]
        public void Render_OneViolationWithOneRecordOfTwoWrongFields_Correct()
        {
            var candidateTable = new DataTable() { TableName = "MyTable" };
            candidateTable.Columns.Add(new DataColumn("Id"));
            candidateTable.Columns.Add(new DataColumn("ForeignKey"));
            candidateTable.Columns.Add(new DataColumn("Numeric value"));
            candidateTable.Columns.Add(new DataColumn("Boolean value"));
            candidateTable.LoadDataRow(new object[] { 1, "Alpha", 10, true }, false);
            candidateTable.LoadDataRow(new object[] { 2, "Beta", 20, false }, false);

            var foreignKeyDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value };
            var booleanDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("Boolean value"), Role = ColumnRole.Value };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { candidateTable.Columns[2] , new LookupMatchesViolationData(false, 15) },
                    { candidateTable.Columns[3] , new LookupMatchesViolationData(false, false) },
                },
            };
            var association = new LookupMatchesViolationComposite(candidateTable.Rows[0], records);

            var msg = new LookupTableHelper(new[] { association }, new[] { foreignKeyDefinition, numericDefinition, booleanDefinition });
            var value = msg.Render().ToMarkdown();

            Console.WriteLine(value);
            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(4));
            Assert.That(value, Is.StringContaining("| 10 <> 15"));
            Assert.That(value, Is.StringContaining("| True <> False"));
        }

        [Test]
        public void Render_OneViolationWithOneRecordOfOneCorrectAndOneWrongField_Correct()
        {
            var candidateTable = new DataTable() { TableName = "MyTable" };
            candidateTable.Columns.Add(new DataColumn("Id"));
            candidateTable.Columns.Add(new DataColumn("ForeignKey"));
            candidateTable.Columns.Add(new DataColumn("Numeric value"));
            candidateTable.Columns.Add(new DataColumn("Boolean value"));
            candidateTable.LoadDataRow(new object[] { 1, "Alpha", 10, true }, false);
            candidateTable.LoadDataRow(new object[] { 2, "Beta", 20, false }, false);

            var foreignKeyDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value, Type = ColumnType.Numeric };
            var booleanDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("Boolean value"), Role = ColumnRole.Value, Type = ColumnType.Boolean };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { candidateTable.Columns[3] , new LookupMatchesViolationData(false, false) },
                },
            };
            var association = new LookupMatchesViolationComposite(candidateTable.Rows[0], records);

            var msg = new LookupTableHelper(new[] { association }, new[] { foreignKeyDefinition, numericDefinition, booleanDefinition });
            var value = msg.Render().ToMarkdown();

            Console.WriteLine(value);
            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(4));
            Assert.That(value, Is.StringContaining("| 10   "));
            Assert.That(value, Is.StringContaining("| True <> False"));
        }

        [Test]
        public void Render_OneViolationWithThreeRecordOfTwoFields_Correct()
        {
            var candidateTable = new DataTable() { TableName = "MyTable" };
            candidateTable.Columns.Add(new DataColumn("Id"));
            candidateTable.Columns.Add(new DataColumn("ForeignKey"));
            candidateTable.Columns.Add(new DataColumn("Numeric value"));
            candidateTable.Columns.Add(new DataColumn("Boolean value"));
            candidateTable.LoadDataRow(new object[] { 1, "Alpha", 10, true }, false);
            candidateTable.LoadDataRow(new object[] { 2, "Beta", 20, false }, false);

            var foreignKeyDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value, Type = ColumnType.Numeric };
            var booleanDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("Boolean value"), Role = ColumnRole.Value, Type = ColumnType.Boolean };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { candidateTable.Columns[2] , new LookupMatchesViolationData(true, "10.0") },
                    { candidateTable.Columns[3] , new LookupMatchesViolationData(false, false) },
                },
                new LookupMatchesViolationRecord()
                {
                    { candidateTable.Columns[2] , new LookupMatchesViolationData(false, "12") },
                    { candidateTable.Columns[3] , new LookupMatchesViolationData(false, false) },
                },
                new LookupMatchesViolationRecord()
                {
                    { candidateTable.Columns[2] , new LookupMatchesViolationData(false, "18") },
                    { candidateTable.Columns[3] , new LookupMatchesViolationData(true, true) },
                },
            };
            var association = new LookupMatchesViolationComposite(candidateTable.Rows[0], records);

            var msg = new LookupTableHelper(new[] { association }, new[] { foreignKeyDefinition, numericDefinition, booleanDefinition });
            var value = msg.Render().ToMarkdown();

            Console.WriteLine(value);
            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(6));
            Assert.That(value, Is.StringContaining("| 10   "));
            Assert.That(value, Is.StringContaining("| True <> False"));
            Assert.That(value, Is.StringContaining("| 10 <> 12  "));
            Assert.That(value, Is.StringContaining("| True <> False"));
            Assert.That(value, Is.StringContaining("| 10 <> 18  "));
            Assert.That(value, Is.StringContaining("| True   "));
        }

        [Test]
        public void Render_TwoViolationsForSameKey_Correct()
        {
            var candidateTable = new DataTable() { TableName = "MyTable" };
            candidateTable.Columns.Add(new DataColumn("Id"));
            candidateTable.Columns.Add(new DataColumn("ForeignKey"));
            candidateTable.Columns.Add(new DataColumn("Numeric value"));
            candidateTable.Columns.Add(new DataColumn("Boolean value"));
            candidateTable.LoadDataRow(new object[] { 1, "Alpha", 10, true }, false);
            candidateTable.LoadDataRow(new object[] { 2, "Alpha", 20, false }, false);

            var foreignKeyDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value, Type = ColumnType.Numeric };
            var booleanDefinition = new Column() { Identifier = new ColumnIdentifierFactory().Instantiate("Boolean value"), Role = ColumnRole.Value, Type = ColumnType.Boolean };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { candidateTable.Columns[2] , new LookupMatchesViolationData(false, "17.0") },
                    { candidateTable.Columns[3] , new LookupMatchesViolationData(false, false) },
                },
                new LookupMatchesViolationRecord()
                {
                    { candidateTable.Columns[2] , new LookupMatchesViolationData(false, "12") },
                    { candidateTable.Columns[3] , new LookupMatchesViolationData(false, false) },
                },
                new LookupMatchesViolationRecord()
                {
                    { candidateTable.Columns[2] , new LookupMatchesViolationData(false, "18") },
                    { candidateTable.Columns[3] , new LookupMatchesViolationData(true, true) },
                },
            };
            var firstAssociation = new LookupMatchesViolationComposite(candidateTable.Rows[0], records);
            var secondAssociation = new LookupMatchesViolationComposite(candidateTable.Rows[1], records);


            var msg = new LookupTableHelper(new[] { firstAssociation, secondAssociation }, new[] { foreignKeyDefinition, numericDefinition, booleanDefinition });
            var value = msg.Render().ToMarkdown();

            Console.WriteLine(value);
            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(9));
            Assert.That(value, Is.StringContaining("| 10 <> 17"));
            Assert.That(value, Is.StringContaining("| True <> False"));
            Assert.That(value, Is.StringContaining("| 10 <> 12  "));
            Assert.That(value, Is.StringContaining("| True <> False"));
            Assert.That(value, Is.StringContaining("| 10 <> 18  "));
            Assert.That(value, Is.StringContaining("| True   "));
            Assert.That(value, Is.StringContaining("| 20 <> 17"));
            Assert.That(value, Is.StringContaining("| 20 <> 12  "));
            Assert.That(value, Is.StringContaining("| 20 <> 18  "));
        }
    }
}
