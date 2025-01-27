using NBi.Core.Etl;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Decoration.DataEngineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Extensibility.Decoration.DataEngineering;

namespace NBi.Core.Decoration.DataEngineering;

public class EtlRunCommandArgs : IDataEngineeringCommandArgs
{
    public EtlRunCommandArgs(Guid guid, string connectionString, IEtlArgs etl)
    {
        Guid = guid;
        ConnectionString = connectionString;
        Etl = etl;
    }

    public Guid Guid { get; set; }
    public string ConnectionString { get; }
    public IEtlArgs Etl { get; }
}
