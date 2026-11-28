using System.Reflection;
using NBi.Core.Assemblies;
using NBi.Core.Compiling;
using NBi.Extensibility.Resolving;
using NBi.CSharp.Assemblies;

namespace NBi.CSharp;

internal sealed class CSharpRuntime : ICSharpRuntime
{
    public Func<object?> CompileScalar(string code)
    {
        var compiler = new DynamicVariableCompiler();
        compiler.Compile(code);
        return compiler.Evaluate;
    }

    public Func<T, object?> CompileTransformation<T>(string code)
    {
        var compiler = new TransformerCompiler<T>();
        compiler.Compile(code);
        return compiler.Evaluate;
    }

    public Type[] DiscoverExtensions(string assemblyName)
        => ExtensionDiscovery.Execute(assemblyName);

    public object CreateInstance(string assemblyPath, string typeName, object[] parameters)
        => AssemblyExecution.GetInstance(assemblyPath, typeName, parameters);

    public Type GetStaticType(string assemblyPath, string typeName)
        => AssemblyExecution.GetStatic(assemblyPath, typeName);

    public object? ExecuteMethod(object target, string methodName, IDictionary<string, object> parameters)
        => AssemblyExecution.Execute(target, methodName, parameters);

    public object? ExecuteStaticMethod(Type type, string methodName, IDictionary<string, object> parameters)
        => AssemblyExecution.ExecuteStatic(type, methodName, parameters);

    public T CreateCustom<T>(ICustomArgs args, string kind, Func<string, Assembly> loadAssembly)
        where T : class
        => CustomFactory<T>.Instantiate(args, kind, loadAssembly);

    public Assembly LoadCustomAssembly(string path)
        => CustomFactory<object>.GetAssembly(path);

    public Type GetCustomType<T>(Assembly assembly, string typeName)
        where T : class
        => CustomFactory<T>.GetType(assembly, typeName);

    public IReadOnlyDictionary<string, object?> ResolveParameters(IReadOnlyDictionary<string, IScalarResolver> parameters)
        => CustomFactory<object>.GetParameters(parameters);

    public T BindConstructor<T>(Type type, IReadOnlyDictionary<string, object?> parameters)
        where T : class
        => CustomFactory<T>.Instantiate(type, parameters);
}
