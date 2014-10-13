using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.WindowsService
{
    class WindowsServiceCheckFactory
    {
        public WindowsServiceCheck Get(IWindowsServiceCheck check)
        {
            if (check is IWindowsServiceRunningCheck)
                return WindowsServiceCheck.IsRunning(check);
                                
            throw new ArgumentException();
        }
    }
}
