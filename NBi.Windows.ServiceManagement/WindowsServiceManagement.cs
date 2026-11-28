using System.Runtime.Versioning;
using System.ServiceProcess;
using NBi.Core.Decoration.Process;

namespace NBi.Windows.ServiceManagement;

internal sealed class WindowsServiceManagement : IServiceManagement
{
    public void Start(string serviceName, int timeout)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Starting a Windows service requires a Windows host.");

        using var service = new ServiceController(serviceName);
        Transition(
            service.Status, ServiceControllerStatus.StartPending, ServiceControllerStatus.Running,
            TimeSpan.FromMilliseconds(timeout), service.Start, service.WaitForStatus);
    }

    public void Stop(string serviceName, int timeout)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Stopping a Windows service requires a Windows host.");

        using var service = new ServiceController(serviceName);
        Transition(
            service.Status, ServiceControllerStatus.StopPending, ServiceControllerStatus.Stopped,
            TimeSpan.FromMilliseconds(timeout), service.Stop, service.WaitForStatus);
    }

    public bool IsRunning(string serviceName, int timeout)
    {
        if (!OperatingSystem.IsWindows())
            throw new PlatformNotSupportedException("Checking a Windows service requires a Windows host.");

        if (!Exists(serviceName))
            return false;

        using var service = new ServiceController(serviceName);
        WaitIfStarting(service.Status, TimeSpan.FromMilliseconds(timeout), service.WaitForStatus);
        return service.Status == ServiceControllerStatus.Running;
    }

    [SupportedOSPlatform("windows")]
    internal static void Transition(
        ServiceControllerStatus status,
        ServiceControllerStatus pending,
        ServiceControllerStatus target,
        TimeSpan timeout,
        Action changeStatus,
        Action<ServiceControllerStatus, TimeSpan> waitForStatus)
    {
        if (status != target && status != pending)
            changeStatus();

        waitForStatus(target, timeout);
    }

    [SupportedOSPlatform("windows")]
    internal static void WaitIfStarting(
        ServiceControllerStatus status,
        TimeSpan timeout,
        Action<ServiceControllerStatus, TimeSpan> waitForStatus)
    {
        if (status == ServiceControllerStatus.StartPending)
            waitForStatus(ServiceControllerStatus.Running, timeout);
    }

    [SupportedOSPlatform("windows")]
    private static bool Exists(string serviceName)
    {
        var services = ServiceController.GetServices();
        try
        {
            return services.Any(service => service.ServiceName.Equals(serviceName));
        }
        finally
        {
            foreach (var service in services)
                service.Dispose();
        }
    }
}
