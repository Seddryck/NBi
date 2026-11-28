using NBi.Core.Configuration;

namespace NBi.Core.Assemblies;

internal static class CSharpRuntime
{
    private static readonly Lazy<ICSharpRuntime> runtime =
        OptionalComponent.Create<ICSharpRuntime>("NBi.CSharp", "NBi.CSharp.CSharpRuntime");

    internal static ICSharpRuntime Instance => runtime.Value;
}
