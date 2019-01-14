using NBi.Core.FlatFile;
using NBi.Core.Query;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility.FlatFile;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    public class FlatFileResultSetResolverArgs : ResultSetResolverArgs
    {
        public IScalarResolver<string> Path { get; }
        public string BasePath { get; }
        public string ParserName { get; }
        public IFlatFileProfile Profile { get; }

        public FlatFileResultSetResolverArgs(IScalarResolver<string> path, string basePath, string parserName, IFlatFileProfile profile)
        {
            this.Path = path;
            this.BasePath = basePath;
            this.ParserName = parserName;
            this.Profile = profile;
        }
    }
}
