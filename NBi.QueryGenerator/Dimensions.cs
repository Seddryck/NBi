using System.Collections.Generic;
using System;

namespace NBi.QueryGenerator
{
    [Serializable]
    public class Dimensions : Dictionary<string, Dimension>
    {
        public void Add(string uniqueName, string caption, string defaultHierarchy)
        {
            this.Add(uniqueName, new Dimension(uniqueName, caption, defaultHierarchy));
        }

        public void Add(Dimension dimension)
        {
            this.Add(dimension.UniqueName, dimension);
        }
    }
}
