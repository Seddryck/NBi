using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource;

class CustomConditionWithTwoParameters : ICustomCondition
{
    public CustomConditionWithTwoParameters(string name, int count)
    { }

    public CustomConditionResult Execute() => CustomConditionResult.SuccessfullCondition;
}
