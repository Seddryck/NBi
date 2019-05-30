using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.WindowsService
{
    class WindowsServiceConditionFactory
    {
        public WindowsServiceCondition Instantiate(IWindowsServiceConditionMetadata check)
        {
            if (check is IWindowsServiceRunningMetadata)
                return WindowsServiceCondition.IsRunning(check);
                                
            throw new ArgumentException();
        }
    }
}
