
using System.Collections.Generic;


namespace NBi.Core.Analysis.Metadata
{

    public class MeasureCollection : Dictionary<string, Measure>
    {
        public void Add(string uniqueName, string caption, string displayFolder)
        {
            this.Add(uniqueName, new Measure(uniqueName, caption, displayFolder));
        }


        public void Add(Measure measure)
        {
            this.Add(measure.UniqueName, measure);
        }


    }


}
