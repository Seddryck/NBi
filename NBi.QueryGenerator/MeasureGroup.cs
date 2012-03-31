using System;

namespace NBi.QueryGenerator
{
    [Serializable]
    public class MeasureGroup
    {
        public string Name { get; private set; }
        public Measures Measures { get; private set; }
        public Dimensions LinkedDimensions { get; private set; }

        public MeasureGroup(string name)
        {
            Name = name;
            Measures = new Measures();
            LinkedDimensions = new Dimensions();
        }

        public MeasureGroup Clone()
        {
            return new MeasureGroup(Name);
        }
    }
}
