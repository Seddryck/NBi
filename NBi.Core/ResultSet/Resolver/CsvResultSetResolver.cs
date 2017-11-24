using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver
{
    class CsvResultSetResolver : IResultSetResolver
    {
        private readonly CsvResultSetResolverArgs args;

        public CsvResultSetResolver(CsvResultSetResolverArgs args)
        {
            this.args = args;
        }

        public virtual ResultSet Execute()
        {
            if (!System.IO.File.Exists(args.Path))
                throw new ExternalDependencyNotFoundException(args.Path);

            var reader = new CsvReader(args.Profile);
            var dataTable = reader.Read(args.Path);

            var rs = new ResultSet();
            rs.Load(dataTable);
            return rs;
        }
    }
}
