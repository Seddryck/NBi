using NBi.Model.Items;
using NBi.Model.Variables.Custom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model.Variables;

public class GlobalVariable
{
    public string Name { get; set; }

    public Script Script { get; set; }

    public QueryScalar QueryScalar { get; set; }

    public EnvironmentVariable Environment { get; set; }

    public CustomVariable Custom { get; set; }
}
