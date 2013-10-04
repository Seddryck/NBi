using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class TableCollection : Dictionary<string, Table>
    {
        public void AddOrIgnore(string name)
        {
            if (!this.ContainsKey(name))
                this.Add(name, new Table(name));
        }

        public void Add(Table table)
        {
            this.Add(table.Name, table);
        }

        public TableCollection Clone()
        {
            var tc = new TableCollection();
            foreach (var t in this)
                tc.Add(t.Value.Clone());
            return tc;
        }
    }
}
