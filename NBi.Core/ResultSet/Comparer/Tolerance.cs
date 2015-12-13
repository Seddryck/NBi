using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
    public abstract class Tolerance
    {
        public virtual string ValueString { get; private set; }
        public SideTolerance Side { get; private set; }


        protected Tolerance(string value, SideTolerance side)
        {
            ValueString = value;
            this.Side = side;
        }
    }
}
