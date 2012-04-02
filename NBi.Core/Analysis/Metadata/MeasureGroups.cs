using System.Collections.Generic;


namespace NBi.Core.Analysis.Metadata
{
    public class MeasureGroups : Dictionary<string, MeasureGroup>
    {
        public void Add(string name)
        {
            this.Add(name, new MeasureGroup(name));
        }

        public void Add(MeasureGroup measureGroup)
        {
            this.Add(measureGroup.Name, measureGroup);
        }
    }
}
