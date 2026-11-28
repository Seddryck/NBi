using System.Reflection;
using NBi.Extensibility.Resolving;

namespace NBi.Core.Assemblies;

internal interface ICSharpRuntime
{
    Func<object?> CompileScalar(string code);
    Func<T, object?> CompileTransformation<T>(string code);
    Type[] DiscoverExtensions(string assemblyName);
    object CreateInstance(string assemblyPath, string typeName, object[] parameters);
    Type GetStaticType(string assemblyPath, string typeName);
    object? ExecuteMethod(object target, string methodName, IDictionary<string, object> parameters);
    object? ExecuteStaticMethod(Type type, string methodName, IDictionary<string, object> parameters);
    T CreateCustom<T>(ICustomArgs args, string kind, Func<string, Assembly> loadAssembly)
        where T : class;
    Assembly LoadCustomAssembly(string path);
    Type GetCustomType<T>(Assembly assembly, string typeName)
        where T : class;
    IReadOnlyDictionary<string, object?> ResolveParameters(IReadOnlyDictionary<string, IScalarResolver> parameters);
    T BindConstructor<T>(Type type, IReadOnlyDictionary<string, object?> parameters)
        where T : class;
}
