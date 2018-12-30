using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation
{
    public interface IBasePredicateInfo
    {
        ColumnType ColumnType { get; set; }
        ComparerType ComparerType { get; }
        bool Not { get; set; }
    }
}
