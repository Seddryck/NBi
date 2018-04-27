using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Lookup
{
    public class ColumnMappingCollectionTest
    {
        [Test]
        public void Add_MixOfNameandIndex_NBiException()
        {
            var mappings = new ColumnMappingCollection();
            mappings.Add(new ColumnMapping("name", "name", ColumnType.Text));
            Assert.Throws<NBiException>(() => mappings.Add(new ColumnMapping("#1", "#1", ColumnType.Text)));
        }

        [Test]
        public void Add_MixOfNameandIndexInOneMapping_NoException()
        {
            var mappings = new ColumnMappingCollection();
            mappings.Add(new ColumnMapping("name", "#0", ColumnType.Text));
            Assert.DoesNotThrow(() => mappings.Add(new ColumnMapping("name2", "#1", ColumnType.Text)));
        }
    }
}
