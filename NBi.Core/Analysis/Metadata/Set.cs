namespace NBi.Core.Analysis.Metadata
{

    public class Set : IField
    {
        public string UniqueName { get; private set; }
        public string Caption { get; set; }

        public Set(string uniqueName, string caption)
        {
            UniqueName = uniqueName;
            Caption = caption;
        }

        public Set Clone()
        {
            return new Set(UniqueName, Caption);
        }

        public override string ToString()
        {
            return Caption.ToString();
        }

    }
}
