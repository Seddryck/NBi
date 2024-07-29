using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.Process.Commands
{
    class StopCommand : IDecorationCommand
    {
        private readonly ServiceStopCommandArgs args;
        public StopCommand(ServiceStopCommandArgs args) => this.args = args;

        public void Execute() => Execute(args.ServiceName.Execute() ?? throw new ArgumentNullException(), args.TimeOut.Execute());

        internal void Execute(string serviceName, int timeOut)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException();

            var service = new ServiceController(serviceName);
            var timeout = TimeSpan.FromMilliseconds(timeOut);

            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
                service.Stop();

            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
        }
    }
}
