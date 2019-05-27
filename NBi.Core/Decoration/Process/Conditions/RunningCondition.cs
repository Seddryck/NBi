using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.Process.Conditions
{
    class RunningCondition : IDecorationCondition
    {
        private readonly IRunningConditionArgs args;
        public RunningCondition(IRunningConditionArgs args) => this.args = args;

        public string Message { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool Validate() => Validate(args.ServiceName.Execute(), args.TimeOut.Execute());

        internal bool Validate(string serviceName, int timeOut)
        {
            //Message = $"Check that the service named '{serviceName}' is running.";

            if (!ServiceController.GetServices().Any(serviceController => serviceController.ServiceName.Equals(serviceName)))
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
}
