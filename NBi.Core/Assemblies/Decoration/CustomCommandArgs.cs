using NBi.Core.Decoration;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using NBi.Extensibility.Decoration;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Assemblies.Decoration;

public class CustomCommandArgs : ICustomArgs, IDecorationCommandArgs
{
    public CustomCommandArgs(Guid guid, IScalarResolver<string> assemblyPath, IScalarResolver<string> typeName, IReadOnlyDictionary<string, IScalarResolver> parameters)
    {
        Guid = guid;
        AssemblyPath = assemblyPath;
        TypeName = typeName;
        Parameters = parameters;
    }

    public Guid Guid { get; set; }

    public IScalarResolver<string> AssemblyPath { get; }

    public IScalarResolver<string> TypeName { get; }

    public IReadOnlyDictionary<string, IScalarResolver> Parameters { get; }
}
