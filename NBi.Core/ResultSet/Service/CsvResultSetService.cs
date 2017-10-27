using NBi.Core.Query;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Service
{
    class CsvResultSetService : IResultSetService
    {
        private readonly string path;
        private readonly CsvProfile profile;

        public CsvResultSetService(string path, CsvProfile profile)
        {
            this.path = path;
            this.profile = profile;
        }

        public virtual ResultSet Execute()
        {
            if (!System.IO.File.Exists(path))
                throw new ExternalDependencyNotFoundException(path);

            var reader = new CsvReader(profile);
            var dataTable = reader.Read(path);

            var rs = new ResultSet();
            rs.Load(dataTable);
            return rs;
        }
    }
}
