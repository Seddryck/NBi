using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Resources;

public class CustomConditionTrue : ICustomCondition
{
    public CustomConditionResult Execute() => CustomConditionResult.SuccessfullCondition;
}
