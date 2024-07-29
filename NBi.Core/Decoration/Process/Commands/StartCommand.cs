using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.Process.Commands
{
    class StartCommand : IDecorationCommand
    {
        private readonly ServiceStartCommandArgs args;
        public StartCommand(ServiceStartCommandArgs args) => this.args = args;

        public void Execute() 
            => Execute(args.ServiceName.Execute() ?? throw new NullReferenceException(), args.TimeOut.Execute());

        internal void Execute(string serviceName, int timeOut)
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                throw new PlatformNotSupportedException();

            var service = new ServiceController(serviceName);
            var timeout = TimeSpan.FromMilliseconds(timeOut);

            if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                service.Start();

            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }
    }
}
