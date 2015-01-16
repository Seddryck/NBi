using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.WindowsService
{
    public interface IWindowsServiceBase
    {
        string ServiceName { get; set; }
        int TimeOut { get; set; }
    }
}
