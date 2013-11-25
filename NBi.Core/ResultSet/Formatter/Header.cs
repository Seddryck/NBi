using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.ResultSet.Comparer;

namespace NBi.Core.ResultSet.Formatter
{
    class Header
    {
        public string FieldName { get; set; }
        public ColumnRole Role { get; set; }
        public ColumnType Type { get; set; }
        public Tolerance Tolerance { get; set; }
        public Rounding Rounding { get; set; }

        public Header()
        {
        }


        internal void Load(System.Data.DataColumn dataColumn)
        {
            Role = (ColumnRole)dataColumn.ExtendedProperties["NBi::Role"];
            Type = (ColumnType)dataColumn.ExtendedProperties["NBi::Type"];
            Tolerance = (Tolerance)dataColumn.ExtendedProperties["NBi::Tolerance"];
            Rounding = (Rounding)dataColumn.ExtendedProperties["NBi::Rounding"];
        }
    }
}
