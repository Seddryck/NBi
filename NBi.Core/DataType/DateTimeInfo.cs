using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType;

public class DateTimeInfo : DataTypeInfo, IPrecision
{
    public int? Precision { get; set; }

    public override string ToString()
    {
        return Name + (Precision.HasValue ? "(" + Precision.Value.ToString() + ")" : "");
    }
}
