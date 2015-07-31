using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType
{
    class TextInfo : DataTypeInfo
    {
        public int Length { get; set; }
        public string CharSet { get; set; }
        public string Collation { get; set; }
        public string Domain { get; set; }
    }
}
