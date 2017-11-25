using NBi.Core.Query;
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
        public string Path { get; }
        public CsvProfile Profile { get; }

        public CsvResultSetResolverArgs(string path, CsvProfile profile)
        {
            this.Path = path;
            this.Profile = profile;
        }
    }
}
