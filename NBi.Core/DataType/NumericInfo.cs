using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType
{
    class NumericInfo : DataTypeInfo
    {
        public int Scale { get; set; }
        public int Precision { get; set; }
    }
}
