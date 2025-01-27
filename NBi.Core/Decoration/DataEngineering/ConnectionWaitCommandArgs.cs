using NBi.Core.Decoration.DataEngineering;
using NBi.Extensibility.Resolving;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.DataEngineering;

public class ConnectionWaitCommandArgs : IDataEngineeringCommandArgs
{
    public ConnectionWaitCommandArgs(Guid guid, string connectionString, IScalarResolver<int> timeOut)
    {
        Guid = guid;
        ConnectionString = connectionString;
        TimeOut = timeOut;
    }

    public Guid Guid { get; set; }
    public string ConnectionString { get; }
    public IScalarResolver<int> TimeOut { get; }
}
