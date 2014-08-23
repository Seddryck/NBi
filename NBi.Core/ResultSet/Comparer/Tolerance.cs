using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.ResultSet.Comparer
{
    public abstract class Tolerance
    {

        public virtual string ValueString { get; private set; }

        protected Tolerance(string value)
        {
            ValueString = value; 
        }
    }
}
