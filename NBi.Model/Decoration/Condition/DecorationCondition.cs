using System;
using System.Linq;
using NBi.Core;

namespace NBi.Model.Decoration.Condition;

public abstract class DecorationCondition
{
    public Settings.SuiteSettings Settings { get; set; }
}
