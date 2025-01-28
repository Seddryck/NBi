using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Extensibility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Lookup;

public class ColumnMappingCollectionTest
{
    [Test]
    public void Add_MixOfNameAndOrdinal_NBiException()
    {
        var mappings = new ColumnMappingCollection
        {
            new ColumnMapping(new ColumnNameIdentifier("name"), ColumnType.Text)
        };
        Assert.Throws<NBiException>(() => mappings.Add(new ColumnMapping(new ColumnOrdinalIdentifier(1), ColumnType.Text)));
    }

    [Test]
    public void Add_MixOfNameAndOrdinalInOneMapping_NoException()
    {
        var mappings = new ColumnMappingCollection();
        Assert.DoesNotThrow(() => mappings.Add(new ColumnMapping(new ColumnNameIdentifier("name"), new ColumnOrdinalIdentifier(1), ColumnType.Text)));
    }


    [Test]
    public void Add_MixOfNameAndOrdinalInSecondMapping_NoException()
    {
        var mappings = new ColumnMappingCollection()
        {
            new ColumnMapping(new ColumnNameIdentifier("zero"), new ColumnOrdinalIdentifier(0), ColumnType.Text)
        };
        Assert.DoesNotThrow(() => mappings.Add(new ColumnMapping(new ColumnNameIdentifier("name"), new ColumnOrdinalIdentifier(1), ColumnType.Text)));
    }
}
