using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class PropertyCollection : Dictionary<string, Property>
    {
        public void AddOrIgnore(string uniqueName, string caption)
        {
            if (!this.ContainsKey(uniqueName))
                this.Add(uniqueName, new Property(uniqueName, caption));
        }

        public void Add(Property property)
        {
            this.Add(property.UniqueName, property);
        }

        public PropertyCollection Clone()
        {
            var props = new PropertyCollection();
            foreach (var prop in this)
                props.Add(prop.Value.Clone());
            return props;
        }
    }
}
