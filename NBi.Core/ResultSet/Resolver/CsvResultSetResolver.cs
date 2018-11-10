using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Scalar.Resolver;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
            var path = args.Path.Execute();
            var file = (Path.IsPathRooted(path)) ? path : args.BasePath + path;

            if (!File.Exists(file))
                throw new ExternalDependencyNotFoundException(file);

            var reader = new CsvReader(args.Profile);
            var dataTable = reader.Read(file);

            var rs = new ResultSet();
            rs.Load(dataTable);
            return rs;
        }
    }
}
