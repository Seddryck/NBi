using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resource;

class CustomConditionWithMulipleCtors : ICustomCondition
{
    public CustomConditionWithMulipleCtors()
    { }

    public CustomConditionWithMulipleCtors(string name)
    { }

    public CustomConditionWithMulipleCtors(string name, int count)
    { }

    public CustomConditionResult Execute() => CustomConditionResult.SuccessfullCondition;
}
