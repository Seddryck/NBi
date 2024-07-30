using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType
{
    public class DataTypeInfo
    {
        public string? Name { get; set; }
        
        public bool Nullable { get; set; }

        public override string ToString()
            => Name ?? string.Empty;
    }
}
