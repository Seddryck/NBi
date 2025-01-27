using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility.Decoration;

public interface ICustomCondition
{
    CustomConditionResult Execute();
}
