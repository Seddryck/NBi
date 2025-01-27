using NBi.Core.Decoration;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Assemblies;

public interface ICustomArgs
{
    IScalarResolver<string> AssemblyPath { get; }
    IScalarResolver<string> TypeName { get; }
    IReadOnlyDictionary<string, IScalarResolver> Parameters { get; }
}
