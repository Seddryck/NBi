﻿using System;
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
using System.IO;
using Newtonsoft.Json;
using NBi.Framework.FailureMessage.Json.Helper;
using NBi.Framework.Sampling;

namespace NBi.Testing.Framework.FailureMessage.Json.Helper
{
    public class LookupTableHelperJsonTest
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

            var foreignKeyDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("ForeignKey"), Role = ColumnRole.Key };
            var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Numeric value"), Role = ColumnRole.Value };

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

            var sampler = new FullSampler<LookupMatchesViolationComposite>();
            sampler.Build(new[] { association });
            var msg = new LookupTableHelperJson(new[] { association }
                , new[] { foreignKeyDefinition, numericDefinition }
                , sampler);
            var sb = new StringBuilder();
            using (var sw = new StringWriter(sb))
            using (var writer = new JsonTextWriter(sw))
            {
                msg.Render(writer);
                var value = sb.ToString();
                Assert.That(value, Does.Contain(",\"rows\":[[\"Alpha\",{\"value\":\"10\",\"expectation\":\"15\"},\"True\"]]}"));
            }
        }
    }
}
