using NBi.Core.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Assemblies
{
    public interface ICustomConditionArgs : IDecorationConditionArgs
    {
        string AssemblyPath { get; }
        string TypeName { get; }
        IReadOnlyDictionary<string, object> Parameters { get; }
    }
}
