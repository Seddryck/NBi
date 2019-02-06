using NBi.Extensibility.Condition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Assemblies.Resource
{
    class CustomConditionWithoutParameter : ICustomCondition
    {
        public CustomConditionWithoutParameter()
        { }

        public CustomConditionResult Execute() => CustomConditionResult.SuccessfullCondition;
    }
}
