using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Model.Decoration.Condition;

namespace NBi.Model.Decoration;

public class TestCondition
{
    public List<DecorationCondition> Predicates { get; set; }

    public TestCondition()
    {
        Predicates = new List<DecorationCondition>();
    }
}
