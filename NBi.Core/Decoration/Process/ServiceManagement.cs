using NBi.Core.Configuration;

namespace NBi.Core.Decoration.Process;

internal static class ServiceManagement
{
    private static readonly Lazy<IServiceManagement> serviceManagement = OptionalComponent.Create<IServiceManagement>(
        "NBi.Windows.ServiceManagement",
        "NBi.Windows.ServiceManagement.WindowsServiceManagement");

    internal static IServiceManagement Instance => serviceManagement.Value;
}
