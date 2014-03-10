using System;
using System.Linq;
using System.ServiceProcess;

namespace NBi.Core.WindowsService
{
    class WindowsServiceCommand: IDecorationCommandImplementation
    {
        public static WindowsServiceCommand Start(IWindowsServiceCommand command)
        {
            var cmd = new WindowsServiceCommand(command.ServiceName, command.TimeOut);
            cmd.Action = cmd.Start;
            return cmd;
        }

        public static WindowsServiceCommand Stop(IWindowsServiceCommand command)
        {
            var cmd = new WindowsServiceCommand(command.ServiceName, command.TimeOut);
            cmd.Action = cmd.Stop;
            return cmd;
        }

        protected WindowsServiceCommand(string serviceName, int timeout)
        {
            ServiceName = serviceName;
            Timeout = timeout;
        }

        public string ServiceName { get; set; }
        public int Timeout { get; set; }
        protected Action Action { get; set; }

        public void Execute()
        {
            Action.Invoke();
        }

        protected void Start()
        {
            var service = new ServiceController(ServiceName);
            var timeout = TimeSpan.FromMilliseconds(Timeout);

            if (service.Status != ServiceControllerStatus.Running && service.Status != ServiceControllerStatus.StartPending)
                service.Start();
            
            service.WaitForStatus(ServiceControllerStatus.Running, timeout);
        }

        protected void Stop()
        {
            var service = new ServiceController(ServiceName);
            var timeout = TimeSpan.FromMilliseconds(Timeout);

            if (service.Status != ServiceControllerStatus.Stopped && service.Status != ServiceControllerStatus.StopPending)
                service.Stop();

            service.WaitForStatus(ServiceControllerStatus.Stopped, timeout);
        }

    }
}
