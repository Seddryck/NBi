using NBi.Core.ResultSet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Variables;

public class DerivedVariable
{
    public string Name { get; set; }
    public string BasedOn { get; set; }
    public ColumnType ColumnType { get; set; } = ColumnType.Text;
    public Script Script { get; set; }
}
