using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.WindowsService
{
    public interface IWindowsServiceConditionMetadata : IDecorationConditionMetadata
    {
        string ServiceName { get; }
        int TimeOut { get; }
    }
}
