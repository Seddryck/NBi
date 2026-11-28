using System.ServiceProcess;
using NBi.Core.Decoration.Process;
using NBi.Windows.ServiceManagement;
using NUnit.Framework;

namespace NBi.Testing.Core.Decoration;

public class WindowsServiceManagementTest
{
#pragma warning disable CA1416 // These tests use status values and synthetic callbacks without calling the Windows service API.
    [TestCase(ServiceControllerStatus.Stopped, ServiceControllerStatus.StartPending, ServiceControllerStatus.Running, true)]
    [TestCase(ServiceControllerStatus.StartPending, ServiceControllerStatus.StartPending, ServiceControllerStatus.Running, false)]
    [TestCase(ServiceControllerStatus.Running, ServiceControllerStatus.StartPending, ServiceControllerStatus.Running, false)]
    [TestCase(ServiceControllerStatus.Running, ServiceControllerStatus.StopPending, ServiceControllerStatus.Stopped, true)]
    [TestCase(ServiceControllerStatus.StopPending, ServiceControllerStatus.StopPending, ServiceControllerStatus.Stopped, false)]
    [TestCase(ServiceControllerStatus.Stopped, ServiceControllerStatus.StopPending, ServiceControllerStatus.Stopped, false)]
    public void Transition_ForwardsTimeoutAndChangesStateOnlyWhenNeeded(
        ServiceControllerStatus initial, ServiceControllerStatus pending, ServiceControllerStatus target, bool changesState)
    {
        var timeout = TimeSpan.FromMilliseconds(42);
        var actions = new List<string>();
        void WaitForStatus(ServiceControllerStatus status, TimeSpan waitTimeout)
        {
            actions.Add("wait");
            Assert.That(status, Is.EqualTo(target));
            Assert.That(waitTimeout, Is.EqualTo(timeout));
        }

        WindowsServiceManagement.Transition(initial, pending, target, timeout, () => actions.Add("change"), WaitForStatus);

        Assert.That(actions, Is.EqualTo(changesState ? new[] { "change", "wait" } : new[] { "wait" }));
    }

    [Test]
    public void Transition_WhenWaitTimesOut_PropagatesException()
    {
        var timeoutException = new System.TimeoutException();
        var exception = Assert.Throws<System.TimeoutException>(
            () => WindowsServiceManagement.Transition(
                ServiceControllerStatus.StartPending, ServiceControllerStatus.StartPending, ServiceControllerStatus.Running,
                TimeSpan.Zero, () => Assert.Fail(), (_, _) => throw timeoutException));

        Assert.That(exception, Is.SameAs(timeoutException));
    }

    [TestCase(ServiceControllerStatus.StartPending, true)]
    [TestCase(ServiceControllerStatus.Running, false)]
    [TestCase(ServiceControllerStatus.Stopped, false)]
    public void WaitIfStarting_WaitsOnlyForStartPending(ServiceControllerStatus initial, bool shouldWait)
    {
        var timeout = TimeSpan.FromMilliseconds(42);
        var waited = false;
        void WaitForStatus(ServiceControllerStatus target, TimeSpan waitTimeout)
        {
            waited = true;
            Assert.That(target, Is.EqualTo(ServiceControllerStatus.Running));
            Assert.That(waitTimeout, Is.EqualTo(timeout));
        }

        WindowsServiceManagement.WaitIfStarting(initial, timeout, WaitForStatus);

        Assert.That(waited, Is.EqualTo(shouldWait));
    }

#pragma warning restore CA1416

    [Test]
    public void ServiceManagement_LoadsWindowsImplementation()
        => Assert.That(ServiceManagement.Instance, Is.TypeOf<WindowsServiceManagement>());

    [Test]
    public void IsRunning_OnWindowsWithUnknownService_ReturnsFalse()
    {
        if (!OperatingSystem.IsWindows())
            Assert.Ignore("This test queries the local Windows service list.");

        Assert.That(ServiceManagement.Instance.IsRunning(Guid.NewGuid().ToString(), 0), Is.False);
    }

    [Test]
    public void ServiceManagement_OnNonWindows_ReportsUnsupportedPlatform()
    {
        if (OperatingSystem.IsWindows())
            Assert.Ignore("This test requires a non-Windows host.");

        var services = ServiceManagement.Instance;

        Assert.That(() => services.Start("any", 0), Throws.TypeOf<PlatformNotSupportedException>());
        Assert.That(() => services.Stop("any", 0), Throws.TypeOf<PlatformNotSupportedException>());
        Assert.That(() => services.IsRunning("any", 0), Throws.TypeOf<PlatformNotSupportedException>());
    }
}
