using System;
using System.Collections.Generic;

namespace NBi.Core.Analysis.Metadata
{

    public class SetCollection : Dictionary<string, Set>
    {
        public ICollection<IField> GetChildStructure()
        {
            Set[] t = (Set[])Array.CreateInstance(typeof(Set), this.Count);
            this.Values.CopyTo(t, 0);
            return (ICollection<IField>)t;
        }

        public void AddOrIgnore(string uniqueName, string caption)
        {
            if (!this.ContainsKey(uniqueName))
                this.Add(uniqueName, new Set(uniqueName, caption));
        }

        public void Add(Set set)
        {
            this.Add(set.UniqueName, set);
        }

        public SetCollection Clone()
        {
            var sets = new SetCollection();
            foreach (var set in this)
                sets.Add(set.Value.Clone());
            return sets;
        }

    }
}
