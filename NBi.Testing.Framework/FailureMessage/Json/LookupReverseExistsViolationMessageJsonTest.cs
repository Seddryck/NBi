﻿using Moq;
using NBi.Core.Configuration;
using NBi.Core.Configuration.FailureReport;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
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

namespace NBi.Testing.Framework.FailureMessage.Json
{
    public class LookupReverseExistsViolationMessageJsonTest
    {
        #region Helpers
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
        #endregion

        [Test]
        public void RenderMessage_FullSamples_Correct()
        {
            var referenceTable = new DataTable() { TableName = "MyTable" };
            referenceTable.Columns.Add(new DataColumn("ForeignKey"));
            referenceTable.Columns.Add(new DataColumn("Numeric value"));
            referenceTable.LoadDataRow(new object[] { "Alpha", 15 }, false);
            referenceTable.LoadDataRow(new object[] { "Beta", 20 }, false);
            referenceTable.LoadDataRow(new object[] { "Delta", 30 }, false);
            referenceTable.LoadDataRow(new object[] { "Epsilon", 40 }, false);

            var candidateTable = new DataTable() { TableName = "MyTable" };
            candidateTable.Columns.Add(new DataColumn("ForeignKey"));
            candidateTable.Columns.Add(new DataColumn("Numeric value"));
            candidateTable.Columns.Add(new DataColumn("Boolean value"));
            candidateTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            candidateTable.LoadDataRow(new object[] { "Gamma", 20, false }, false);

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value };

            var keyMappings = new ColumnMappingCollection() { new ColumnMapping(foreignKeyDefinition.Identifier, ColumnType.Text) };
            var valueMappings = new ColumnMappingCollection() { new ColumnMapping(numericDefinition.Identifier, ColumnType.Numeric) };

            var violations = new LookupExistsViolationCollection(keyMappings);
            violations.Register(new KeyCollection(new[] { "Gamma" }), candidateTable.Rows[1]);

            var samplers = new Dictionary<string, ISampler<DataRow>>()
            {
                { "candidate", new FullSampler<DataRow>() },
                { "reference", new FullSampler<DataRow>() },
                { "analysis", new FullSampler<DataRow>() },
            };

            var message = new LookupReverseExistsViolationMessageJson(samplers);
            message.Generate(referenceTable.Rows.Cast<DataRow>(), candidateTable.Rows.Cast<DataRow>(), violations, keyMappings, valueMappings);

            var text = message.RenderMessage();
            Assert.That(text, Does.Contain("\"expected\":{\"total-rows\":4,\"table\""));
            Assert.That(text, Does.Contain("\"actual\":{\"total-rows\":2,\"table\""));
            Assert.That(text, Does.Contain("\"analysis\":{\"missing\":{\"total-rows\":1,"));
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
            referenceTable.LoadDataRow(new object[] { "Epsilon", 40 }, false);

            var candidateTable = new DataTable() { TableName = "MyTable" };
            candidateTable.Columns.Add(new DataColumn("ForeignKey"));
            candidateTable.Columns.Add(new DataColumn("Numeric value"));
            candidateTable.Columns.Add(new DataColumn("Boolean value"));
            candidateTable.LoadDataRow(new object[] { "Alpha", 10, true }, false);
            candidateTable.LoadDataRow(new object[] { "Gamma", 20, false }, false);

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value };

            var keyMappings = new ColumnMappingCollection() { new ColumnMapping(foreignKeyDefinition.Identifier, ColumnType.Text) };

            var violations = new LookupExistsViolationCollection(keyMappings);
            violations.Register(new KeyCollection(new[] { "Gamma" }), candidateTable.Rows[1]);

            var samplers = new Dictionary<string, ISampler<DataRow>>()
            {
                { "candidate", new NoneSampler<DataRow>() },
                { "reference", new NoneSampler<DataRow>() },
                { "analysis", new NoneSampler<DataRow>() },
            };

            var message = new LookupReverseExistsViolationMessageJson(samplers);
            message.Generate(referenceTable.Rows.Cast<DataRow>(), candidateTable.Rows.Cast<DataRow>(), violations, keyMappings, null);

            var text = message.RenderMessage();
            Assert.That(text, Does.Contain("\"expected\":{\"total-rows\":4}"));
            Assert.That(text, Does.Contain("\"actual\":{\"total-rows\":2}"));
            Assert.That(text, Does.Contain("\"analysis\":{\"missing\":{\"total-rows\":1}"));
        }
    }
}
