using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using NUnit.Framework;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage.Markdown.Helper;
using MarkdownLog;
using NBi.Framework.FailureMessage.Json.Helper;
using System.IO;
using Newtonsoft.Json;
using NBi.Framework.Sampling;
using NBi.Extensibility;

namespace NBi.Framework.Testing.FailureMessage.Json.Helper;

public class StandardTableHelperJsonTest
{
    [Test]
    public void Build_TwoRows_5Lines()
    {
        var dataTable = new DataTable() { TableName = "MyTable" };
        dataTable.Columns.Add(new DataColumn("Id"));
        dataTable.Columns.Add(new DataColumn("Numeric value"));
        dataTable.Columns.Add(new DataColumn("Boolean value"));
        dataTable.LoadDataRow(["Alpha", 10, true], false);
        dataTable.LoadDataRow(["Beta", 20, false], false);
        var rs = new DataTableResultSet(dataTable);

        var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("Id"), Role = ColumnRole.Key };

        var sampler = new FullSampler<IResultRow>();
        sampler.Build(rs.Rows);
        var msg = new StandardTableHelperJson(rs.Rows
            , [idDefinition]
            , sampler);
        var sb = new StringBuilder();
        using var sw = new StringWriter(sb);
        using var writer = new JsonTextWriter(sw);
        msg.Render(writer);
        var value = sb.ToString();
        Assert.That(sb.ToString, Does.Contain("\"total-rows\":2"));
        Assert.That(sb.ToString, Does.Contain("\"table\":{\"columns\":[{"));
        Assert.That(sb.ToString, Does.Contain("{\"position\":0,\"name\":\"Id\",\"role\":\"KEY\",\"type\":\"Text\"}"));
        Assert.That(sb.ToString, Does.Contain("\"rows\":[[\"Alpha\",\"10\",\"True\"],["));
    }

    [Test]
    public void Build_TwoRowsByOrdinal_FirstRow()
    {
        var dataTable = new DataTable() { TableName = "MyTable" };
        dataTable.Columns.Add(new DataColumn("Id"));
        dataTable.Columns.Add(new DataColumn("Numeric value"));
        dataTable.Columns.Add(new DataColumn("Boolean value"));
        dataTable.LoadDataRow(["Alpha", 10, true], false);
        dataTable.LoadDataRow(["Beta", 20, false], false);
        var rs = new DataTableResultSet(dataTable);

        var idDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#0"), Role = ColumnRole.Key };
        var numericDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#1"), Role = ColumnRole.Value, Type = ColumnType.Numeric };
        var booleanDefinition = new ColumnMetadata() { Identifier = new ColumnIdentifierFactory().Instantiate("#2"), Role = ColumnRole.Value, Type = ColumnType.Boolean };

        var sampler = new FullSampler<IResultRow>();
        sampler.Build(rs.Rows);
        var msg = new StandardTableHelperJson(rs.Rows
            , [idDefinition, numericDefinition, booleanDefinition]
            , sampler);
        var sb = new StringBuilder();
        using var sw = new StringWriter(sb);
        using var writer = new JsonTextWriter(sw);
        msg.Render(writer);
        var value = sb.ToString();
        Assert.That(sb.ToString, Does.Contain("{\"position\":0,\"name\":\"Id\",\"role\":\"KEY\",\"type\":\"Text\"}"));
        Assert.That(sb.ToString, Does.Contain("{\"position\":1,\"name\":\"Numeric value\",\"role\":\"VALUE\",\"type\":\"Numeric\"}"));
        Assert.That(sb.ToString, Does.Contain("{\"position\":2,\"name\":\"Boolean value\",\"role\":\"VALUE\",\"type\":\"Boolean\"}"));
    }
}
