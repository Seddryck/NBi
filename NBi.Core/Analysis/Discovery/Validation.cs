using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Discovery
{
    internal abstract class Validation
    {
        internal abstract void Apply();
        internal abstract void GenerateException();
    }
}
