using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.WindowsService
{
    class WindowsServiceCommandFactory
    {
        public WindowsServiceCommand Get(IWindowsServiceCommand command)
        {
            if (command is IWindowsServiceStartCommand)
                return WindowsServiceCommand.Start(command);
                                
            if (command is IWindowsServiceStopCommand)
                return WindowsServiceCommand.Stop(command);

            throw new ArgumentException();
        }
    }
}
