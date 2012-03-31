using System.Collections.Generic;
using System;

namespace NBi.QueryGenerator
{
    [Serializable]
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
