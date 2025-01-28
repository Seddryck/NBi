using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable;

public interface IVariable
{
    object? GetValue();
    bool IsEvaluated();
}
interface ILoadtimeVariable : IVariable { }

interface IRuntimeVariable : IVariable 
{
    void Evaluate();
}

