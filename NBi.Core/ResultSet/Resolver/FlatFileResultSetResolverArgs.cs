using NBi.Extensibility.Resolving;
using NBi.Extensibility.FlatFile;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

public class FlatFileResultSetResolverArgs : ResultSetResolverArgs
{
    public IScalarResolver<string> Path { get; }
    public IResultSetResolver? Redirection { get; }
    public string BasePath { get; }
    public string ParserName { get; }
    public IFlatFileProfile Profile { get; }

    public FlatFileResultSetResolverArgs(IScalarResolver<string> path, string basePath, string parserName, IResultSetResolver? redirection, IFlatFileProfile profile)
    {
        Path = path;
        BasePath = basePath;
        ParserName = parserName;
        Redirection = redirection;
        Profile = profile;
    }

    public FlatFileResultSetResolverArgs(IScalarResolver<string> resolverPath, string basePath, string parserName, IFlatFileProfile csvProfile)
        : this(resolverPath, basePath, parserName, null, csvProfile) { }
}
