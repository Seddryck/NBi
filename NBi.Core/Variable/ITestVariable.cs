using NBi.Core.Transformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Variable
{
    public interface ITestVariable
    {
        void Evaluate();
        object GetValue();
        bool IsEvaluated();
    }
}
