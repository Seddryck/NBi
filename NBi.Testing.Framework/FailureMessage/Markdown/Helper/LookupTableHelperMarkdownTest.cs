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
using MarkdownLog;
using NBi.Framework.Sampling;

namespace NBi.Framework.Testing.FailureMessage.Markdown.Helper
{
    public class LookupTableHelperMarkdownTest
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
            var rsCandidate = new DataTableResultSet(candidateTable);

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value };

            var keyMappings = new ColumnMappingCollection() { new ColumnMapping(foreignKeyDefinition.Identifier, ColumnType.Text) };
            var valueMappings = new ColumnMappingCollection() { new ColumnMapping(numericDefinition.Identifier, ColumnType.Numeric) };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { rsCandidate.GetColumn(1) ?? throw new NullReferenceException(), new LookupMatchesViolationData(false, 15) },
                },
            };
            var association = new LookupMatchesViolationComposite(rsCandidate.Rows.ElementAt(0), records);

            var sampler = new FullSampler<LookupMatchesViolationComposite>();
            sampler.Build(new[] { association });
            var msg = new LookupTableHelperMarkdown(new[] { association }
                , new[] { foreignKeyDefinition, numericDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();

            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(6));

            var indexes = value.IndexOfAll('\n').ToArray();
            var dashLine = value.Substring(indexes[3] + 1, indexes[4] - indexes[3] - 2);
            Assert.That(dashLine.Distinct().Count(), Is.EqualTo(3));
            Assert.That(dashLine.Distinct(), Has.Member(' '));
            Assert.That(dashLine.Distinct(), Has.Member('-'));
            Assert.That(dashLine.Distinct(), Has.Member('|'));
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
            var rsCandidate = new DataTableResultSet(candidateTable);

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value };
            var booleanDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Boolean value"), Role = ColumnRole.Value };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { rsCandidate.GetColumn(2) ?? throw new NullReferenceException(), new LookupMatchesViolationData(false, 15) },
                    { rsCandidate.GetColumn(3) ?? throw new NullReferenceException(), new LookupMatchesViolationData(false, false) },
                },
            };
            var association = new LookupMatchesViolationComposite(rsCandidate.Rows.ElementAt(0), records);

            var sampler = new FullSampler<LookupMatchesViolationComposite>();
            sampler.Build(new[] { association });
            var msg = new LookupTableHelperMarkdown(new[] { association }
                , new[] { foreignKeyDefinition, numericDefinition, booleanDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();
            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(6));
            Assert.That(value, Does.Contain("| 10 <> 15"));
            Assert.That(value, Does.Contain("| True <> False"));
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
            var rsCandidate = new DataTableResultSet(candidateTable);

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value, Type = ColumnType.Numeric };
            var booleanDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Boolean value"), Role = ColumnRole.Value, Type = ColumnType.Boolean };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { rsCandidate.GetColumn(3) ?? throw new NullReferenceException(), new LookupMatchesViolationData(false, false) },
                },
            };
            var association = new LookupMatchesViolationComposite(rsCandidate.Rows.ElementAt(0), records);

            var sampler = new FullSampler<LookupMatchesViolationComposite>();
            sampler.Build(new[] { association });
            var msg = new LookupTableHelperMarkdown(new[] { association }
                , new[] { foreignKeyDefinition, numericDefinition, booleanDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();
            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(6));
            Assert.That(value, Does.Contain("| 10   "));
            Assert.That(value, Does.Contain("| True <> False"));
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
            var rsCandidate = new DataTableResultSet(candidateTable);

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value, Type = ColumnType.Numeric };
            var booleanDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Boolean value"), Role = ColumnRole.Value, Type = ColumnType.Boolean };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { rsCandidate.GetColumn(2)! , new LookupMatchesViolationData(true, "10.0") },
                    { rsCandidate.GetColumn(3)! , new LookupMatchesViolationData(false, false) },
                },
                new LookupMatchesViolationRecord()
                {
                    { rsCandidate.GetColumn(2)! , new LookupMatchesViolationData(false, "12") },
                    { rsCandidate.GetColumn(3)! , new LookupMatchesViolationData(false, false) },
                },
                new LookupMatchesViolationRecord()
                {
                    { rsCandidate.GetColumn(2)! , new LookupMatchesViolationData(false, "18") },
                    { rsCandidate.GetColumn(3)! , new LookupMatchesViolationData(true, true) },
                },
            };
            var association = new LookupMatchesViolationComposite(rsCandidate.Rows.ElementAt(0), records);

            var sampler = new FullSampler<LookupMatchesViolationComposite>();
            sampler.Build(new[] { association });
            var msg = new LookupTableHelperMarkdown(new[] { association }
                , new[] { foreignKeyDefinition, numericDefinition, booleanDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();

            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(8));
            Assert.That(value, Does.Contain("| 10   "));
            Assert.That(value, Does.Contain("| True <> False"));
            Assert.That(value, Does.Contain("| 10 <> 12  "));
            Assert.That(value, Does.Contain("| True <> False"));
            Assert.That(value, Does.Contain("| 10 <> 18  "));
            Assert.That(value, Does.Contain("| True   "));
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
            var rsCandidate = new DataTableResultSet(candidateTable);

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value, Type = ColumnType.Numeric };
            var booleanDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Boolean value"), Role = ColumnRole.Value, Type = ColumnType.Boolean };

            var records = new List<LookupMatchesViolationRecord>()
            {
                new LookupMatchesViolationRecord()
                {
                    { rsCandidate.GetColumn(2)! , new LookupMatchesViolationData(false, "17.0") },
                    { rsCandidate.GetColumn(3)! , new LookupMatchesViolationData(false, false) },
                },
                new LookupMatchesViolationRecord()
                {
                    { rsCandidate.GetColumn(2)! , new LookupMatchesViolationData(false, "12") },
                    { rsCandidate.GetColumn(3)! , new LookupMatchesViolationData(false, false) },
                },
                new LookupMatchesViolationRecord()
                {
                    { rsCandidate.GetColumn(2)! , new LookupMatchesViolationData(false, "18") },
                    { rsCandidate.GetColumn(3)! , new LookupMatchesViolationData(true, true) },
                },
            };
            var firstAssociation = new LookupMatchesViolationComposite(rsCandidate.Rows.ElementAt(0), records);
            var secondAssociation = new LookupMatchesViolationComposite(rsCandidate.Rows.ElementAt(1), records);

            var sampler = new FullSampler<LookupMatchesViolationComposite>();
            sampler.Build(new[] { firstAssociation, secondAssociation });
            var msg = new LookupTableHelperMarkdown(new[] { firstAssociation, secondAssociation }
                , new[] { foreignKeyDefinition, numericDefinition, booleanDefinition }
                , sampler);
            var container = new MarkdownContainer();
            msg.Render(container);
            var value = container.ToMarkdown();

            Assert.That(value.Count(c => c == '\n'), Is.EqualTo(11));
            Assert.That(value, Does.Contain("Result-set with 2 rows"));
            Assert.That(value, Does.Contain("#0 (Id) | #1 (ForeignKey) | #2 (Numeric value) | #3 (Boolean value)"));
            Assert.That(value, Does.Contain("1       | Alpha           | 10 <> 17           | True <> False"));
            Assert.That(value, Does.Contain(">>      | >>              | 10 <> 12           | True <> False"));
            Assert.That(value, Does.Contain(">>      | >>              | 10 <> 18           | True"));
            Assert.That(value, Does.Contain("2       | Alpha           | 20 <> 17           | False <> False"));
            Assert.That(value, Does.Contain(">>      | >>              | 20 <> 12           | False <> False"));
            Assert.That(value, Does.Contain(">>      | >>              | 20 <> 18           | False"));
        }
    }
}
