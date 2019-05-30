using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Assemblies
{
    public interface ICustomConditionMetadata : IDecorationConditionMetadata
    {
        string AssemblyPath { get; }
        string TypeName { get; }
        IReadOnlyDictionary<string, object> Parameters { get; }
    }
}
