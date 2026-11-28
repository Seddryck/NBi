using System.Reflection;
using System.Runtime.Loader;

namespace NBi.Core.Configuration;

internal static class OptionalComponent
{
    internal static Lazy<T> Create<T>(string assemblyName, string typeName)
        => new(() => Load<T>(assemblyName, typeName));

    internal static T Load<T>(string assemblyName, string typeName)
    {
        var context = AssemblyLoadContext.GetLoadContext(typeof(OptionalComponent).Assembly)!;
        Assembly assembly;
        try
        {
            assembly = context.LoadFromAssemblyName(new AssemblyName(assemblyName));
        }
        catch (FileNotFoundException ex)
        {
            throw new InvalidOperationException(
                $"This feature requires {assemblyName}.dll. Install the {assemblyName} NuGet package in the NBi runner.",
                ex);
        }

        return (T)Activator.CreateInstance(assembly.GetType(typeName, throwOnError: true)!)!;
    }
}
