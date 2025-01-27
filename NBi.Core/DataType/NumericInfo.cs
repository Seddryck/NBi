using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType;

class NumericInfo : DataTypeInfo, IPrecision, IScale
{
    public int? Scale { get; set; }
    public int? Precision { get; set; }

    public override string ToString()
    {
        return Name
            + (Precision.HasValue ? "(" + Precision.Value.ToString() : "")
            + (Scale.HasValue ? "," + Scale.Value.ToString() : "")
            + (Precision.HasValue ? ")" : "");
    }
}
