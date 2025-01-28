using NBi.Extensibility.Resolving;
using NBi.Core.Decoration.DataEngineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.DataEngineering;

public class TableTruncateCommandArgs : IDataEngineeringCommandArgs
{
    public TableTruncateCommandArgs(Guid guid, string connectionString, IScalarResolver<string> tableName)
    {
        Guid = guid;
        ConnectionString = connectionString;
        TableName = tableName;
    }
    public Guid Guid { get; set; }
    public string ConnectionString { get; }
    public IScalarResolver<string> TableName { get; set; }
}
