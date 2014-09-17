using System.Collections.Generic;


namespace NBi.Core.Analysis.Metadata
{
    public class PerspectiveCollection : Dictionary<string, Perspective>
    {
        public void AddOrIgnore(string name)
        {
            if(!this.ContainsKey(name))
                this.Add(name, new Perspective(name));
        }

        public void Add(Perspective perspective)
        {
            this.Add(perspective.Name, perspective);
        }

        public PerspectiveCollection Clone()
        {
            var ps = new PerspectiveCollection();
            foreach (var p in this)
                ps.Add(p.Value.Clone());
            return ps;
        }
    }
}
