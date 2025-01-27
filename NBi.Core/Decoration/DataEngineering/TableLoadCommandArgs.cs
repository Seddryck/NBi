using NBi.Extensibility.Resolving;
using NBi.Core.Decoration.DataEngineering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.DataEngineering;

public class TableLoadCommandArgs : IDataEngineeringCommandArgs
{
    public TableLoadCommandArgs(Guid guid, string connectionString, IScalarResolver<string> tableName, IScalarResolver<string> fileName)
    {
        Guid = guid;
        ConnectionString = connectionString;
        TableName = tableName;
        FileName = fileName;
    }

    public Guid Guid { get; set; }
    public string ConnectionString { get; }
    public IScalarResolver<string> TableName { get; set; }
    public IScalarResolver<string> FileName { get; }
}
