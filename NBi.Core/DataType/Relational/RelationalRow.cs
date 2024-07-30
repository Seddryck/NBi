using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.DataType.Relational
{
    public class RelationalRow
    {
        public string? IsNullable { get; set; }
        public string? DataType { get; set; }
        public int CharacterMaximumLength { get; set; }
        public int NumericPrecision { get; set; }
        public int NumericScale { get; set; }
        public int DateTimePrecision { get; set; }
        public string? CharacterSetName { get; set; }
        public string? CollationName { get; set; }
        public string? DomainName { get; set; }
        
    }
}
