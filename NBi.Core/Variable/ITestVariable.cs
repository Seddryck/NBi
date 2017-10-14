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
        object GetValue();
        bool IsEvaluated();

        LanguageType Language { get; }
        string Code { get; }
    }
}
