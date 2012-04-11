using System.Collections.Generic;

namespace NBi.Core.Analysis.Metadata
{

    public class DimensionCollection : Dictionary<string, Dimension>
    {
        public void AddOrIgnore(string uniqueName, string caption, string defaultHierarchy)
        {
            if (!this.ContainsKey(uniqueName))
                this.Add(uniqueName, new Dimension(uniqueName, caption, defaultHierarchy));
        }

        public void Add(Dimension dimension)
        {
            this.Add(dimension.UniqueName, dimension);
        }

        public DimensionCollection Clone()
        {
            var dims = new DimensionCollection();
            foreach (var dim in this)
            {
                dims.Add(dim.Value.Clone());
            }
            return dims;
        }
    }
}
