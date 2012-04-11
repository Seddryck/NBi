using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class CubeMetadata
    {
        public PerspectiveCollection Perspectives { get; private set; }

        public CubeMetadata()
        {
            Perspectives = new PerspectiveCollection();
        }

        public CubeMetadata Clone()
        {
            var m = new CubeMetadata();
            foreach (var p in Perspectives)
                m.Perspectives.Add(p.Key, p.Value.Clone());
            return m;
        }

        public int GetCountMembers()
        {
            int total = 0;

            foreach (var p in this.Perspectives)
                foreach (var mg in p.Value.MeasureGroups)
                    foreach (var dim in mg.Value.LinkedDimensions)
                        total += mg.Value.Measures.Count * dim.Value.Hierarchies.Count;

            return total;
        }
    }
}
