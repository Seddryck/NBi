using NBi.Core.ResultSet.Combination;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using Rs = NBi.Core.ResultSet;

namespace NBi.Core.Testing.ResultSet.Combination;

public class CartesianProductSequenceCombinationTest
{
    private (IResultSet rs, ISequenceResolver resolver) Initialize()
    {
        var dataTable = new DataTable() { TableName = "MyTable" };
        dataTable.Columns.Add(new DataColumn("Id"));
        dataTable.Columns.Add(new DataColumn("Numeric value"));
        dataTable.Columns.Add(new DataColumn("Boolean value"));
        for (int i = 0; i < 20; i++)
            dataTable.LoadDataRow(["Alpha", i, true], false);
        dataTable.AcceptChanges();
        var rs = new Rs.DataTableResultSet(dataTable);
        
        var scalarResolvers = new List<IScalarResolver>()
        {
            new LiteralScalarResolver<string>("2015-01-01"),
            new LiteralScalarResolver<string>("2016-01-01"),
            new LiteralScalarResolver<string>("2017-01-01"),
        };
        var args = new ListSequenceResolverArgs(scalarResolvers);
        var resolver = new ListSequenceResolver<DateTime>(args);
        return (rs, resolver);
    }

    [Test()]
    public void Execute_TwentyRowsAndSequenceOfTwo_SixtyRows()
    {
        var (rs, resolver) = Initialize();
        var combination = new CartesianProductSequenceCombination(resolver);
        combination.Execute(rs);

        Assert.That(rs.Rows.Count, Is.EqualTo(60));
    }

    [Test()]
    public void Execute_TwentyRowsAndSequenceOfTwo_OneAdditionalColumn()
    {
        var (rs, resolver) = Initialize();
        var initColumnCount = rs.ColumnCount;
        var combination = new CartesianProductSequenceCombination(resolver);
        combination.Execute(rs);

        Assert.That(rs.ColumnCount, Is.EqualTo(initColumnCount + 1));
    }

    [Test()]
    public void Execute_TwentyRowsAndSequenceOfZero_EmptyResultSet()
    {
        var rs = Initialize().rs;
        var initColumnCount = rs.ColumnCount;

        var resolver = new ListSequenceResolver<DateTime>(new ListSequenceResolverArgs([]));
        var combination = new CartesianProductSequenceCombination(resolver);
        combination.Execute(rs);

        Assert.That(rs.ColumnCount, Is.EqualTo(initColumnCount + 1));
        Assert.That(rs.Rows.Count, Is.EqualTo(0));
    }

    [Test()]
    public void Execute_EmptyResultSetAndSequenceOfTwo_EmptyResultSet()
    {
        var (rs, resolver) = Initialize();
        rs.Clear();
        rs.AcceptChanges();
        var initColumnCount = rs.ColumnCount;

        var combination = new CartesianProductSequenceCombination(resolver);
        combination.Execute(rs);

        Assert.That(rs.ColumnCount, Is.EqualTo(initColumnCount + 1));
        Assert.That(rs.Rows.Count, Is.EqualTo(0));
    }
}
