namespace NBi.Core.Analysis.Metadata
{
    public class Hierarchy
    {
        public string UniqueName { get; private set; }
        public string Caption { get; set; }

        public Hierarchy(string uniqueName, string caption)
        {
            UniqueName = uniqueName;
            Caption = caption;
        }

        public Hierarchy Clone()
        {
            return new Hierarchy(UniqueName, Caption);
        }

    }
}
