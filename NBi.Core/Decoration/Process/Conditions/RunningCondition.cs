using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.Process.Conditions;

class RunningCondition : IDecorationCondition
{
    private readonly IRunningConditionArgs args;
    public RunningCondition(IRunningConditionArgs args) => this.args = args;

    public string Message { get => $"Check that the service named '{args.ServiceName.Execute()}' is running."; }

    public bool Validate() 
        => Validate(args.ServiceName.Execute() ?? throw new NullReferenceException(), args.TimeOut.Execute());

    internal bool Validate(string serviceName, int timeOut)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            throw new PlatformNotSupportedException();

        if (!ServiceController.GetServices().Any(sc => RuntimeInformation.IsOSPlatform(OSPlatform.Windows) && sc.ServiceName.Equals(serviceName)))
            return false;

        var service = new ServiceController(serviceName);

        //If current status is starting then wait for X milliseconds and then execute the check.
        if (service.Status == ServiceControllerStatus.StartPending)
        {
            var timeout = TimeSpan.FromMilliseconds(timeOut);
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }
        return service.Status == ServiceControllerStatus.Running;
    }


}
