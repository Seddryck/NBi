using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Process;

public class ServiceStopCommandArgs : IProcessCommandArgs
{
    public ServiceStopCommandArgs(Guid guid, IScalarResolver<string> serviceName, IScalarResolver<int> timeOut)
    {
        Guid = guid;
        ServiceName = serviceName;
        TimeOut = timeOut;
    }

    public Guid Guid { get; set; }
    public IScalarResolver<string> ServiceName { get; }
    public IScalarResolver<int> TimeOut { get; }
}
