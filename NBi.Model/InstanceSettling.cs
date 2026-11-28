using NBi.Model.Settings;
using NBi.Model.Variables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Model;

public class InstanceSettling
{
    public InstanceVariable Variable { get; set; }

    public List<DerivedVariable> DerivedVariables { get; set; } = new List<DerivedVariable>();

    public List<string> Categories { get; set; } = new List<string>();

    public List<Trait> Traits { get; set; } = new List<Trait>();

    public static InstanceSettling Unique { get; } = new InstanceUnique();

    public class InstanceUnique : InstanceSettling
    { }
}
