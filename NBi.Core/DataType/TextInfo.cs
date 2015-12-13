using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType
{
    class TextInfo : DataTypeInfo, ILength
    {
        public int? Length { get; set; }
        public string CharSet { get; set; }
        public string Collation { get; set; }
        public string Domain { get; set; }

        public override string ToString()
        {
            return Name + (Length.HasValue ? "(" + Length.Value.ToString() + ")" : "");
        }
    }
}
