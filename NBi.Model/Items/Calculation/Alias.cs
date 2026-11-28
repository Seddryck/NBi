using System;
using System.Linq;
using NBi.Core.Evaluate;

namespace NBi.Model.Items.Calculation;

public class Alias : IColumnAlias
{
    public int Column { get; set; }

    public string Name { get; set; }

}
