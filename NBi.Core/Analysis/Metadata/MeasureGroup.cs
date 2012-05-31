namespace NBi.Core.Analysis.Metadata
{

    public class MeasureGroup
    {
        public string Name { get; private set; }
        public Measures Measures { get; private set; }
        public DimensionCollection LinkedDimensions { get; private set; }

        public MeasureGroup(string name)
        {
            Name = name;
            Measures = new Measures();
            LinkedDimensions = new DimensionCollection();
        }

        public MeasureGroup Clone()
        {
            return new MeasureGroup(Name);
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
