namespace NBi.Core.Analysis.Metadata
{
    public class Hierarchy : IField
    {
        public string UniqueName { get; private set; }
        public string Caption { get; set; }
        public LevelCollection Levels { get; set; }

        public Hierarchy(string uniqueName, string caption)
        {
            UniqueName = uniqueName;
            Caption = caption;
            Levels = new LevelCollection();
        }

        protected Hierarchy(string uniqueName, string caption, LevelCollection levels)
        {
            UniqueName = uniqueName;
            Caption = caption;
            Levels = levels;
        }

        public Hierarchy Clone()
        {
            return new Hierarchy(UniqueName, Caption, Levels.Clone());
        }

        public override string ToString()
        {
            return Caption.ToString();
        }

    }
}
