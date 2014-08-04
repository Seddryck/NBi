namespace NBi.Core.Analysis.Metadata
{

    public class MeasureGroup : IField
    {
        public string Name { get; private set; }
        public MeasureCollection Measures { get; private set; }
        public DimensionCollection LinkedDimensions { get; private set; }

        public MeasureGroup(string name)
        {
            Name = name;
            Measures = new MeasureCollection();
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

        public string Caption
        {
            get
            {
                return Name;
            }
            set
            {
                Name = value;
            }
        }
    }
}
