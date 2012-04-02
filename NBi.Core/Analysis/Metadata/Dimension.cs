namespace NBi.Core.Analysis.Metadata
{

    public class Dimension
    {
        public string UniqueName { get; private set; }
        public string Caption { get; set; }
        public Hierarchies Hierarchies { get; set; }

        public Dimension(string uniqueName, string caption, string defaultHierarchyUniqueName)
        {
            UniqueName = uniqueName;
            Caption = caption;
            Hierarchies = new Hierarchies();
            var defaultHierarchy = new Hierarchy(defaultHierarchyUniqueName, null);
            Hierarchies.AddDefault(defaultHierarchy);
        }

        public Dimension(string uniqueName, string caption, Hierarchies hierarchies)
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
