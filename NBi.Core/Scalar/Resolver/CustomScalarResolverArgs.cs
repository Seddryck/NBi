﻿using NBi.Core.Assemblies;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver;

public class CustomScalarResolverArgs : IScalarResolverArgs, ICustomArgs
{
    public IScalarResolver<string> AssemblyPath { get; }

    public IScalarResolver<string> TypeName { get; }

    public IReadOnlyDictionary<string, IScalarResolver> Parameters { get; }

    public CustomScalarResolverArgs(IScalarResolver<string> assemblyPath, IScalarResolver<string> typeName, IDictionary<string, IScalarResolver> parameters)
        => (AssemblyPath, TypeName, Parameters) = (assemblyPath, typeName, new ReadOnlyDictionary<string, IScalarResolver>(parameters));
}
