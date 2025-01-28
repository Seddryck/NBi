using NBi.Core.Scalar.Comparer;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet;

public class ColumnMetadata
{
    public IColumnIdentifier? Identifier { get; set; }
    public ColumnRole Role { get; set; }
    public ColumnType Type { get; set; }
    public Tolerance? Tolerance { get; set; }
    public Rounding? Rounding { get; set; }
}
