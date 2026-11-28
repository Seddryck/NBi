using NBi.Core.Assemblies;

namespace NBi.Core.Configuration.Extension;

public class ExtensionAnalyzer
{
    public Type[] Execute(string name) => CSharpRuntime.Instance.DiscoverExtensions(name);
}
