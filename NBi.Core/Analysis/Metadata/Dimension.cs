namespace NBi.Core.Analysis.Metadata
{

    public class Dimension
    {
        public string UniqueName { get; private set; }
        public string Caption { get; set; }
        public HierarchyCollection Hierarchies { get; set; }

        public Dimension(string uniqueName, string caption, string defaultHierarchyUniqueName)
        {
            UniqueName = uniqueName;
            Caption = caption;
            Hierarchies = new HierarchyCollection();
            //var defaultHierarchy = new Hierarchy(defaultHierarchyUniqueName, null);
            //Hierarchies.AddDefault(defaultHierarchy);
        }

        public Dimension(string uniqueName, string caption, HierarchyCollection hierarchies)
        {
            UniqueName = uniqueName;
            Caption = caption;
            Hierarchies = hierarchies;
        }

        public Dimension Clone()
        {
            return new Dimension(UniqueName, Caption, Hierarchies.Clone());
        }

    }
}
