using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Emit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Compiling;

internal abstract class CSharpCompiler : IDisposable
{
    private bool disposed = false;
    private MemoryStream? CompilationResult { get; set; }
    private MethodInfo? Method { get; set; }

    protected abstract string Template { get; }
    protected abstract string[] TemplateVariables { get; }
    protected string Namespace => $"{GetType().Namespace}.Dynamic";

    public virtual void Compile(string code)
    {
        var substitution = string.Format(Template, TemplateVariables.Prepend(code).ToArray());
        var syntaxTree = CSharpSyntaxTree.ParseText(substitution, new CSharpParseOptions(LanguageVersion.CSharp8));
        var basePath = Path.GetDirectoryName(typeof(System.Runtime.GCSettings).GetTypeInfo().Assembly.Location)
                        ?? throw new InvalidOperationException();

        var root = syntaxTree.GetRoot() as CompilationUnitSyntax;
        var references = root!.Usings;
        List<string> referencePaths =
        [
            typeof(object).GetTypeInfo().Assembly.Location,
            typeof(Console).GetTypeInfo().Assembly.Location,
            Path.Combine(basePath, "System.Runtime.dll"),
            Path.Combine(basePath, "System.Runtime.Extensions.dll"),
            Path.Combine(basePath, "mscorlib.dll"),
            Path.Combine(basePath, "System.Xml.dll"),
            Path.Combine(basePath, "System.Xml.XDocument.dll"),
            Path.Combine(basePath, "System.Linq.dll"),
            Path.Combine(basePath, "System.Xml.XPath.dll"),
            Path.Combine(basePath, "System.Xml.XDocument.dll"),
            Path.Combine(basePath, "System.Private.Xml.Linq.dll"),
            Path.Combine(basePath, "System.Private.Xml.dll")
        ];
        referencePaths.AddRange(references.Select(x => Path.Combine(basePath, $"{x.Name}.dll")));
        referencePaths = referencePaths.Distinct().ToList();

        var executableReferences = new List<PortableExecutableReference>();
        foreach (var reference in referencePaths)
        {
            if (File.Exists(reference))
                executableReferences.Add(MetadataReference.CreateFromFile(reference));
        }

        var compilation = CSharpCompilation.Create(Path.GetRandomFileName())
            .AddSyntaxTrees([syntaxTree])
            .AddReferences(executableReferences)
            .WithOptions(new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));
        CompilationResult = new MemoryStream();
        var compilationResult = compilation.Emit(CompilationResult);
        if (!compilationResult.Success)
        {
            var errors = compilationResult.Diagnostics.Where(
                            diagnostic =>
                                diagnostic.IsWarningAsError ||
                                diagnostic.Severity == DiagnosticSeverity.Error
                            )?.ToList() ?? [];
            throw new AggregateException(errors.Select(x => new Exception(x.GetMessage())));
        }
    }

    protected void Prepare(string typeName, string methodName)
    {
        if (CompilationResult is null)
            throw new InvalidOperationException();

        CompilationResult.Seek(0, SeekOrigin.Begin);
        var assemblyContext = new AssemblyLoadContext(Path.GetRandomFileName(), true);
        var assembly = assemblyContext.LoadFromStream(CompilationResult);
        var type = assembly.GetType($"{Namespace}.{typeName}", true, true);
        Method = type!.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
        assemblyContext.Unload();
    }

    protected object? Evaluate(params object?[] values)
    {
        if (Method is null)
            throw new InvalidOperationException();

        var result = Method!.Invoke(null, BindingFlags.InvokeMethod, Type.DefaultBinder, values, null);
        return result;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposed)
            return;

        if (disposing)
            CompilationResult?.Dispose();

        disposed = true;
    }
}
