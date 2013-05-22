using System.Collections.Generic;


namespace NBi.Core.Analysis.Metadata
{
    public class MeasureGroupCollection : Dictionary<string, MeasureGroup>
    {
        public void AddOrIgnore(string name)
        {
            if (!this.ContainsKey(name))
                this.Add(name, new MeasureGroup(name));
        }

        public void Add(MeasureGroup measureGroup)
        {
            this.Add(measureGroup.Name, measureGroup);
        }

        public MeasureGroupCollection Clone()
        {
            var mgs = new MeasureGroupCollection();
            foreach (var mg in this)
            {
                mgs.Add(mg.Value.Clone());
            }
            return mgs;
        }
    }
}
