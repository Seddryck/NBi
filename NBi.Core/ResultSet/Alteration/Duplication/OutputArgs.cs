using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Duplication
{
    public class OutputArgs
    {
        public OutputValue Value { get; set; }
        public string Name { get; set; }

        public OutputArgs(string name, OutputValue value)
            => (Name, Value) = (name, value);
    }

    public enum OutputValue
    {
        Index = 0,
        Total = 1
    }
}
