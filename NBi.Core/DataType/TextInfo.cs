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
        public string CharSet { get; set; } = string.Empty;
        public string Collation { get; set; } = string.Empty;
        public string Domain { get; set; } = string.Empty;

        public override string ToString()
            => $"{Name}{(Length.HasValue ? '(' + Length.Value.ToString() + ')' : string.Empty)}";
    }
}
