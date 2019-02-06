using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Condition
{
    public interface ICustomCondition
    {
        CustomConditionResult Execute();
    }
}
