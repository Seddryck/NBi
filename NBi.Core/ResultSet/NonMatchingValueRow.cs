using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace NBi.Core.ResultSet
{
    public class NonMatchingValueRow
    {
        public DataRow Actual { get; set; }
        public DataRow Expected { get; set; }

        public void t()
        {
        }
    }
}
