using System.Collections.Generic;


namespace NBi.Core.Analysis.Metadata
{
    public class HierarchyCollection : Dictionary<string, Hierarchy>
    {
        protected string defaultUniqueName;

        //public Hierarchy Default { get { return this[defaultUniqueName]; } }

        public void AddOrIgnore(string uniqueName, string caption)
        {
            if (this.ContainsKey(uniqueName))
                this[uniqueName].Caption=caption;
            else
                this.Add(uniqueName, new Hierarchy(uniqueName, caption));
        }

        //public void AddDefault(Hierarchy hierarchy)
        //{
        //    this.Add(hierarchy);
        //    defaultUniqueName= hierarchy.UniqueName;
        //}

        public void Add(Hierarchy hierarchy)
        {
            this.Add(hierarchy.UniqueName, hierarchy);
        }

        public HierarchyCollection Clone()
        {
            return new HierarchyCollection();
        }

    }
}
