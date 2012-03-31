using System;
using System.Collections.Generic;


namespace NBi.QueryGenerator
{

    public class Measures : Dictionary<string, Measure>
    {
        public void Add(string uniqueName, string caption)
        {
            this.Add(uniqueName, new Measure(uniqueName, caption));
        }


        public void Add(Measure measure)
        {
            this.Add(measure.UniqueName, measure);
        }


    }


}
