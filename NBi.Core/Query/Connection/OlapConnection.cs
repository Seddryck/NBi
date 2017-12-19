using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Connection
{
    class OlapConnection : IConnection
    {
        public string ConnectionString { get; }

        public OlapConnection(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public object CreateNew() => CreateConnection();

        public Microsoft.AnalysisServices.AdomdClient.AdomdConnection CreateConnection()
        {
            return new Microsoft.AnalysisServices.AdomdClient.AdomdConnection(ConnectionString);
        }

    }
}
