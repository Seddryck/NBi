using NBi.Core.FlatFile;
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
    class FlatFileResultSetResolver : IResultSetResolver
    {
        private readonly FlatFileResultSetResolverArgs args;

        public FlatFileResultSetResolver(FlatFileResultSetResolverArgs args)
        {
            this.args = args;
        }

        public virtual ResultSet Execute()
        {
            var path = args.Path.Execute();
            var file = (Path.IsPathRooted(path)) ? path : args.BasePath + path;

            if (!File.Exists(file))
                throw new ExternalDependencyNotFoundException(file);

            var factory = new FlatFileReaderFactory();
            var reader = factory.Instantiate(args.ParserName, args.Profile);
            var dataTable = reader.ToDataTable(file);

            var rs = new ResultSet();
            rs.Load(dataTable);
            return rs;
        }
    }
}
