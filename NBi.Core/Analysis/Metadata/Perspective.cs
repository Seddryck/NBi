using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Metadata
{
    public class Perspective
    {
        public string Name { get; private set; }
        public MeasureGroupCollection MeasureGroups { get; private set; }
        public DimensionCollection Dimensions { get; private set; }

        public Perspective(string name)
        {
            Name = name;
            MeasureGroups = new MeasureGroupCollection();
            Dimensions = new DimensionCollection();
        }

        public Perspective Clone()
        {
            var p = new Perspective(Name);
            p.MeasureGroups = MeasureGroups.Clone();
            p.Dimensions = Dimensions.Clone();
            return p;
        }

        public ICollection<IElement> GetChildStructure()
        {
            return Dimensions.GetChildStructure();
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
