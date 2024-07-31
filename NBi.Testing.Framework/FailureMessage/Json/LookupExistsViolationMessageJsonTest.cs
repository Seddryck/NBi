using Moq;
using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using NBi.Framework;
using NBi.Framework.FailureMessage;
using NBi.Framework.FailureMessage.Json;
using NBi.Framework.Sampling;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Testing.FailureMessage.Json
{
    public class LookupExistsViolationMessageJsonTest
    {
        [Test]
        public void RenderMessage_FullSamples_Correct()
        {
            var referenceTable = new DataTable() { TableName = "MyTable" };
            referenceTable.Columns.Add(new DataColumn("ForeignKey"));
            referenceTable.Columns.Add(new DataColumn("Numeric value"));
            referenceTable.LoadDataRow(new object[] { "Alpha", 15 }, false);
            referenceTable.LoadDataRow(new object[] { "Beta", 20 }, false);
            referenceTable.LoadDataRow(new object[] { "Delta", 30 }, false);
            var rsReference = new DataTableResultSet(referenceTable);

            var candidateTable = new DataTable() { TableName = "MyTable" };
            candidateTable.Columns.Add(new DataColumn("ForeignKey"));
            candidateTable.Columns.Add(new DataColumn("Numeric value"));
            candidateTable.Columns.Add(new DataColumn("Boolean value"));
            candidateTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            candidateTable.LoadDataRow(new object[] { "Gamma", 20, false }, false);
            var rsCandidate = new DataTableResultSet(candidateTable);

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value };

            var keyMappings = new ColumnMappingCollection() { new ColumnMapping(foreignKeyDefinition.Identifier, ColumnType.Text) };
            var valueMappings = new ColumnMappingCollection() { new ColumnMapping(numericDefinition.Identifier, ColumnType.Numeric) };

            var violations = new LookupExistsViolationCollection(keyMappings);
            violations.Register(new KeyCollection(new[] { "Gamma" }), rsCandidate.Rows.ElementAt(1));

            var samplers = new Dictionary<string, ISampler<IResultRow>>()
            {
                { "candidate", new FullSampler<IResultRow>() },
                { "reference", new FullSampler<IResultRow>() },
                { "analysis", new FullSampler<IResultRow>() },
            };

            var message = new LookupExistsViolationMessageJson(samplers);
            message.Generate(rsReference.Rows, rsCandidate.Rows, violations, keyMappings, valueMappings);

            var text = message.RenderMessage();
            Assert.That(text, Does.Contain("\"actual\":{\"total-rows\":2,\"table\""));
            Assert.That(text, Does.Contain("\"expected\":{\"total-rows\":3,\"table\""));
            Assert.That(text, Does.Contain("\"analysis\":{\"unexpected\":{\"total-rows\":1,"));
            Assert.That(text, Does.Contain("[[\"Gamma\",\"20\",\"False\"]]"));
        }

        [Test]
        public void RenderMessage_NoneSamples_Correct()
        {
            var referenceTable = new DataTable() { TableName = "MyTable" };
            referenceTable.Columns.Add(new DataColumn("ForeignKey"));
            referenceTable.Columns.Add(new DataColumn("Numeric value"));
            referenceTable.LoadDataRow(new object[] { "Alpha", 15 }, false);
            referenceTable.LoadDataRow(new object[] { "Beta", 20 }, false);
            referenceTable.LoadDataRow(new object[] { "Delta", 30 }, false);
            var rsReference = new DataTableResultSet(referenceTable);

            var candidateTable = new DataTable() { TableName = "MyTable" };
            candidateTable.Columns.Add(new DataColumn("ForeignKey"));
            candidateTable.Columns.Add(new DataColumn("Numeric value"));
            candidateTable.Columns.Add(new DataColumn("Boolean value"));
            candidateTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            candidateTable.LoadDataRow(new object[] { "Gamma", 20, false }, false);
            var rsCandidate = new DataTableResultSet(candidateTable);

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value };

            var keyMappings = new ColumnMappingCollection() { new ColumnMapping(foreignKeyDefinition.Identifier, ColumnType.Text) };

            var violations = new LookupExistsViolationCollection(keyMappings);
            violations.Register(new KeyCollection(new[] { "Gamma" }), rsCandidate.Rows.ElementAt(1));

            var samplers = new Dictionary<string, ISampler<IResultRow>>()
            {
                { "candidate", new NoneSampler<IResultRow>() },
                { "reference", new NoneSampler<IResultRow>() },
                { "analysis", new NoneSampler<IResultRow>() },
            };

            var message = new LookupExistsViolationMessageJson(samplers);
            message.Generate(rsReference.Rows, rsCandidate.Rows, violations, keyMappings, []);

            var text = message.RenderMessage();
            Assert.That(text, Does.Contain("\"actual\":{\"total-rows\":2}"));
            Assert.That(text, Does.Contain("\"expected\":{\"total-rows\":3}"));
            Assert.That(text, Does.Contain("\"analysis\":{\"unexpected\":{\"total-rows\":1}"));
        }
    }
}
