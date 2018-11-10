using NBi.Core.Query;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    public class CsvResultSetResolverArgs : ResultSetResolverArgs
    {
        public IScalarResolver<string> Path { get; }
        public string BasePath { get; }
        public CsvProfile Profile { get; }

        public CsvResultSetResolverArgs(IScalarResolver<string> path, string basePath, CsvProfile profile)
        {
            this.Path = path;
            this.BasePath = basePath;
            this.Profile = profile;
        }
    }
}
