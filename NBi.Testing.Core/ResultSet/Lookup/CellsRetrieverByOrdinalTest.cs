using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Extensibility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Lookup;

public class CellsRetrieverByOrdinalTest
{
    internal IResultSet BuildDataTable(object[] keys, object[] secondKeys, object[] values)
    {
        var ds = new DataSet();
        var dt = ds.Tables.Add("myTable");

        var keyCol = dt.Columns.Add("myKey");
        var secondKeyCol = dt.Columns.Add("mySecondKey");
        var valueCol = dt.Columns.Add("myValue");

        for (int i = 0; i < keys.Length; i++)
        {
            var dr = dt.NewRow();
            dr.SetField(keyCol, keys[i]);
            dr.SetField(secondKeyCol, secondKeys[i]);
            dr.SetField(valueCol, values[i]);
            dt.Rows.Add(dr);
        }

        return new DataTableResultSet(dt);
    }

    [Test]
    public void GetKeys_UniqueCell_CorrectCell()
    {
        var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, [0, 1, 0]);

        var columns = new List<IColumnDefinition>()
        {
            new Column(new ColumnOrdinalIdentifier(0), ColumnRole.Key, ColumnType.Text)
        };

        var keyRetriever = new CellRetrieverByOrdinal(columns);
        Assert.That(keyRetriever.GetColumns(table[0]).Members, Is.EqualTo(new[] { "Key0" }));
        Assert.That(keyRetriever.GetColumns(table[1]).Members, Is.EqualTo(new[] { "Key1" }));
        Assert.That(keyRetriever.GetColumns(table[2]).Members, Is.EqualTo(new[] { "Key0" }));
    }

    [Test]
    public void GetKeys_UniqueCellNumeric_CorrectCell()
    {
        var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, [0, 1, 0]);

        var columns = new List<IColumnDefinition>()
        {
            new Column(new ColumnOrdinalIdentifier(2), ColumnRole.Key, ColumnType.Numeric)
        };

        var keyRetriever = new CellRetrieverByOrdinal(columns);
        Assert.That(keyRetriever.GetColumns(table[0]).Members, Is.EqualTo(new[] { 0 }));
        Assert.That(keyRetriever.GetColumns(table[1]).Members, Is.EqualTo(new[] { 1 }));
        Assert.That(keyRetriever.GetColumns(table[2]).Members, Is.EqualTo(new[] { 0 }));
    }

    [Test]
    public void GetKeys_UniqueCellNumericCasting_CorrectCell()
    {
        var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, ["0", "1.0", "0.00"]);

        var columns = new List<IColumnDefinition>()
        {
            new Column(new ColumnOrdinalIdentifier(2), ColumnRole.Key, ColumnType.Numeric)
        };

        var keyRetriever = new CellRetrieverByOrdinal(columns);
        Assert.That(keyRetriever.GetColumns(table[0]).Members, Is.EqualTo(new[] { 0 }));
        Assert.That(keyRetriever.GetColumns(table[1]).Members, Is.EqualTo(new[] { 1 }));
        Assert.That(keyRetriever.GetColumns(table[2]).Members, Is.EqualTo(new[] { 0 }));
    }

    [Test]
    public void GetKeys_TwoCells_CorrectCells()
    {
        var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, [0, 1, 0]);

        var columns = new List<IColumnDefinition>()
        {
            new Column(new ColumnOrdinalIdentifier(0), ColumnRole.Key, ColumnType.Text),
            new Column(new ColumnOrdinalIdentifier(1), ColumnRole.Key, ColumnType.Text)
        };

        var keyRetriever = new CellRetrieverByOrdinal(columns);
        Assert.That(keyRetriever.GetColumns(table[0]).Members, Is.EqualTo(new[] { "Key0", "Foo" }));
        Assert.That(keyRetriever.GetColumns(table[1]).Members, Is.EqualTo(new[] { "Key1", "Bar" }));
        Assert.That(keyRetriever.GetColumns(table[2]).Members, Is.EqualTo(new[] { "Key0", "Foo" }));
    }

    [Test]
    public void GetKeys_TwoCellsDifferentTypes_CorrectCells()
    {
        var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, [0, 1, 0]);

        var columns = new List<IColumnDefinition>()
        {
            new Column(new ColumnOrdinalIdentifier(0), ColumnRole.Key, ColumnType.Text),
            new Column(new ColumnOrdinalIdentifier(2), ColumnRole.Key, ColumnType.Numeric)
        };

        var keyRetriever = new CellRetrieverByOrdinal(columns);
        Assert.That(keyRetriever.GetColumns(table[0]).Members, Is.EqualTo(new object[] { "Key0", 0 }));
        Assert.That(keyRetriever.GetColumns(table[1]).Members, Is.EqualTo(new object[] { "Key1", 1 }));
        Assert.That(keyRetriever.GetColumns(table[2]).Members, Is.EqualTo(new object[] { "Key0", 0 }));
    }

    [Test]
    public void GetKeys_TwoCellsReverseOrder_CorrectCells()
    {
        var table = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, [0, 1, 0]);

        var columns = new List<IColumnDefinition>()
        {
            new Column(new ColumnOrdinalIdentifier(1), ColumnRole.Key, ColumnType.Text),
            new Column(new ColumnOrdinalIdentifier(0), ColumnRole.Key, ColumnType.Text)
        };

        var keyRetriever = new CellRetrieverByOrdinal(columns);
        Assert.That(keyRetriever.GetColumns(table[0]).Members, Is.EqualTo(new[] { "Foo", "Key0" }));
        Assert.That(keyRetriever.GetColumns(table[1]).Members, Is.EqualTo(new[] { "Bar", "Key1" }));
        Assert.That(keyRetriever.GetColumns(table[2]).Members, Is.EqualTo(new[] { "Foo", "Key0" }));
    }

}

