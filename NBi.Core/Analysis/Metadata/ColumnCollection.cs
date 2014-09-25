using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class ColumnCollection : Dictionary<string, Column>
    {
        public void AddOrIgnore(string name)
        {
            if (!this.ContainsKey(name))
                this.Add(name, new Column(name));
        }

        public void Add(Column column)
        {
            this.Add(column.Name, column);
        }

        public ColumnCollection Clone()
        {
            var collection = new ColumnCollection();
            foreach (var element in this)
                collection.Add(element.Value.Clone());
            return collection;
        }
    }
}
