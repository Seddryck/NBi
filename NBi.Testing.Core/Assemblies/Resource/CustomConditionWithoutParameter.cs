using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource;

class CustomConditionWithoutParameter : ICustomCondition
{
    public CustomConditionWithoutParameter()
    { }

    public CustomConditionResult Execute() => CustomConditionResult.SuccessfullCondition;
}
