using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using NBi.Extensibility.Resolving;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using NBi.Core.Compiling;

namespace NBi.Core.Scalar.Resolver;

class CSharpScalarResolver<T> : IScalarResolver<T>
{
    private CSharpScalarResolverArgs Args { get; }
    private DynamicVariableCompiler? Compiler { get; set; }

    public CSharpScalarResolver(CSharpScalarResolverArgs args)
        => (Args) = (args);

    public CSharpScalarResolver(string code)
        : this(new CSharpScalarResolverArgs(code))
    { }

    public T? Execute()
    {
        if (Compiler is null)
        {
            Compiler = new DynamicVariableCompiler();
            Compiler.Compile(Args.Code);
        }
        var value = Compiler.Evaluate();
        return (T?)Convert.ChangeType(value, typeof(T));
    }

    object? IResolver.Execute() => Execute();
}
