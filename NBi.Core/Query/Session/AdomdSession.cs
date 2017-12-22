using Microsoft.AnalysisServices.AdomdClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Session
{
    class AdomdSession : ISession
    {
        public string ConnectionString { get; }

        public Type UnderlyingSessionType => typeof(AdomdConnection);

        public AdomdSession(string connectionString)
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
