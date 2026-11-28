using NBi.Core.Scalar.Projection;
using NBi.Model.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Items;

public class ProjectionOld
{
    public ProjectionType Type { get; set; }

    public ResultSetSystem ResultSet { get; set; }

    [Obsolete("Replaced by result-set")]
    public ResultSetSystem ResultSetOld
    {
        get => ResultSet;
        set { ResultSet = value; }
    }
}
