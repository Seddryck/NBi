using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Request;

internal abstract class Validation
{
    
    protected Validation()
    {
    }

    internal abstract void Apply();
    internal abstract void GenerateException();
}
