using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.DataEngineering;

public class SqlBatchRunCommandArgs : IDataEngineeringCommandArgs
{
    public IScalarResolver<string> Name { get; }
    public IScalarResolver<string> Path { get; }
    public string BasePath { get; }
    public IScalarResolver<string> Version { get; }
    public string ConnectionString { get; }
    public Guid Guid { get; set; }

    public SqlBatchRunCommandArgs(Guid guid, string connectionString, IScalarResolver<string> name, IScalarResolver<string> path, string basePath, IScalarResolver<string> version)
        => (Guid, ConnectionString, Name, Path, BasePath, Version) = (guid, connectionString, name, path, basePath, version);
}
