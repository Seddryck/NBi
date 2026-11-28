using System.Reflection;
using NBi.Extensibility.Resolving;

namespace NBi.Core.Assemblies;

public class AbstractCustomFactory<T>
    where T : class
{
    protected virtual string CustomKind { get; } = "Unspecified";

    protected T Instantiate(ICustomArgs args)
        => CSharpRuntime.Instance.CreateCustom<T>(args, CustomKind, GetAssembly);

    protected internal virtual Assembly GetAssembly(string path)
        => CSharpRuntime.Instance.LoadCustomAssembly(path);

    protected internal Type GetType(Assembly assembly, string typeName)
        => CSharpRuntime.Instance.GetCustomType<T>(assembly, typeName);

    protected internal IReadOnlyDictionary<string, object?> GetParameters(IReadOnlyDictionary<string, IScalarResolver> parameters)
        => CSharpRuntime.Instance.ResolveParameters(parameters);

    protected internal T Instantiate(Type customCommandType, IReadOnlyDictionary<string, object?> parameters)
        => CSharpRuntime.Instance.BindConstructor<T>(customCommandType, parameters);

    protected internal class TypeNotExistingException : ArgumentException
    {
    }

    protected internal class TypeNotImplementingInterfaceException : ArgumentException
    {
    }

    protected internal class NoConstructorFoundException : ArgumentException
    {
    }
}
