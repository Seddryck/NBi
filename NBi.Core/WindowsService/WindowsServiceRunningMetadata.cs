using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.WindowsService
{
    public class WindowsServiceRunningMetadata : IWindowsServiceRunningMetadata
    {
        public string ServiceName { get; }
        public int TimeOut { get; }

        public WindowsServiceRunningMetadata(string serviceName, int timeOut)
            => (ServiceName, TimeOut) = (serviceName, timeOut);
    }
}
