using System;
using System.Linq;
using NBi.Model.Items;

namespace NBi.Model.Constraints;

public class LinkedTo : AbstractConstraint
{
    public AbstractItem Item { get; set; }
}
