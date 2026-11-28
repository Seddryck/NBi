using NBi.Core.Configuration;
using NBi.Core.Configuration.Extension;
using NBi.Core.FlatFile;
using NUnit.Framework;

namespace NBi.Testing.Core.Assemblies;

public class CSharpRuntimeTest
{
    [Test]
    public void DiscoverExtensions_ConfiguredAssembly_FindsPublicReader()
    {
        var extensions = new ExtensionAnalyzer().Execute(typeof(CsvReader).Assembly.GetName().Name!);

        Assert.That(extensions, Does.Contain(typeof(CsvReader)));
    }

    [Test]
    public void Load_MissingAssembly_IdentifiesPackageToInstall()
    {
        const string assemblyName = "NBi.MissingOptionalComponent";

        var exception = Assert.Throws<InvalidOperationException>(
            () => _ = OptionalComponent.Create<object>(assemblyName, "Unused").Value);

        Assert.That(exception!.Message, Does.Contain($"{assemblyName}.dll").And.Contain($"Install the {assemblyName} NuGet package"));
    }
}
