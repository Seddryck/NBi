namespace NBi.Core.Assemblies;

public class AssemblyManager
{
    public virtual object GetInstance(string assemblyPath, string typeName, object[] ctorParameters)
        => CSharpRuntime.Instance.CreateInstance(assemblyPath, typeName, ctorParameters);

    public virtual Type GetStatic(string assemblyPath, string typeName)
        => CSharpRuntime.Instance.GetStaticType(assemblyPath, typeName);

    public virtual object? Execute(object target, string methodName, IDictionary<string, object> parameters)
        => CSharpRuntime.Instance.ExecuteMethod(target, methodName, parameters);

    public virtual object? ExecuteStatic(Type type, string methodName, IDictionary<string, object> parameters)
        => CSharpRuntime.Instance.ExecuteStaticMethod(type, methodName, parameters);
}
