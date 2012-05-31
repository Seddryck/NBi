using System;
using System.Collections.Generic;

namespace NBi.Core.Analysis.Metadata
{

    public class DimensionCollection : Dictionary<string, Dimension>
    {
        public ICollection<IElement> GetChildStructure()
        {
            Dimension[] t = (Dimension[])Array.CreateInstance(typeof(Dimension), this.Count);
            this.Values.CopyTo(t, 0);
            return (ICollection<IElement>)t;
        }

        public void AddOrIgnore(string uniqueName, string caption)
        {
            if (!this.ContainsKey(uniqueName))
                this.Add(uniqueName, new Dimension(uniqueName, caption));
        }

        public void Add(Dimension dimension)
        {
            this.Add(dimension.UniqueName, dimension);
        }

        public DimensionCollection Clone()
        {
            var dims = new DimensionCollection();
            foreach (var dim in this)
                dims.Add(dim.Value.Clone());
            return dims;
        }

    }
}
