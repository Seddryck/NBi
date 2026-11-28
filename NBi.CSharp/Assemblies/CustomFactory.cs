using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using NBi.Core.Assemblies;

namespace NBi.CSharp.Assemblies;

internal static class CustomFactory<T>
    where T : class
{
    internal static T Instantiate(ICustomArgs args, string kind, Func<string, Assembly> loadAssembly)
    {
        var assembly = loadAssembly(args.AssemblyPath.Execute() ?? throw new InvalidOperationException());
        var parameters = GetParameters(args.Parameters);
        var typeName = args.TypeName.Execute() ?? throw new InvalidOperationException();
        try
        {
            var type = GetType(assembly, typeName);
            return Instantiate(type, parameters);
        }
        catch (AbstractCustomFactory<T>.TypeNotExistingException)
        { throw new NBiException($"The assembly '{assembly.FullName}' doesn't contain any type named '{args.TypeName}'. This type was describe in the test as a {kind}."); }
        catch (AbstractCustomFactory<T>.TypeNotImplementingInterfaceException)
        { throw new NBiException($"The type '{typeName}' of the assembly '{assembly.FullName}' is not implementing the interface '{typeof(T).Name}' but is used as a {kind}."); }
        catch (AbstractCustomFactory<T>.NoConstructorFoundException)
        { throw new NBiException($"The type '{typeName}' of the assembly '{assembly.FullName}' has no constructor matching with the {parameters.Count} parameter{(parameters.Count > 1 ? "s" : string.Empty)} that {(parameters.Count > 1 ? "were" : "was")} provided."); }
    }

    internal static Assembly GetAssembly(string path)
    {
        var assemblyPath = Path.IsPathRooted(path)
            ? path
            : Path.GetFullPath(path);

        if (!File.Exists(assemblyPath))
            throw new ExternalDependencyNotFoundException(assemblyPath);

        var assembly = Assembly.LoadFile(assemblyPath);
        return assembly;
    }

    internal static Type GetType(Assembly assembly, string typeName)
    {
        var type = (typeName.Contains('.')
            ? assembly.GetType(typeName, false, true)
            : assembly.GetTypes().FirstOrDefault(x => x.Name == typeName)) ?? throw new AbstractCustomFactory<T>.TypeNotExistingException();
        if (!type.GetInterfaces().Contains(typeof(T)))
            throw new AbstractCustomFactory<T>.TypeNotImplementingInterfaceException();
        return type;
    }

    internal static IReadOnlyDictionary<string, object?> GetParameters(IReadOnlyDictionary<string, IScalarResolver> parameters)
        => (parameters?.Select(x => new { x.Key, Value = x.Value.Execute() }) ?? [])
                .ToDictionary(x => x.Key, y => y.Value);

    internal static T Instantiate(Type customCommandType, IReadOnlyDictionary<string, object?> parameters)
    {
        var ctor = customCommandType.GetConstructors().FirstOrDefault(
            c => c.GetParameters().All(p => (parameters ?? new Dictionary<string, object?>()).Keys.Contains(p.Name, StringComparer.InvariantCultureIgnoreCase))
            && c.GetParameters().Length == (parameters ?? new Dictionary<string, object?>()).Count
        ) ?? throw new AbstractCustomFactory<T>.NoConstructorFoundException();
        var typeConverter = new TypeConverter();
        var ctorParams = ctor.GetParameters().Select(
            p => typeConverter.Convert(
                parameters.First(x => string.Compare(x.Key, p.Name, true) == 0).Value ?? throw new NullReferenceException()
                , p.ParameterType)
            ).ToArray();
        var instance = ctor.Invoke(ctorParams) as T;

        return instance ?? throw new NullReferenceException();
    }
}
