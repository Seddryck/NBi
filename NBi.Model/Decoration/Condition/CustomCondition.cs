using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Model.Decoration.Condition;

public class CustomCondition : DecorationCondition
{
    public string AssemblyPath { get; set; }

    public string TypeName { get; set; }

    public List<CustomConditionParameter> Parameters { get; set; } = new List<CustomConditionParameter>();
}
