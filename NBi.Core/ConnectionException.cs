using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core
{
    public class ConnectionException : System.Exception
    {
        public ConnectionException(ArgumentException ex)
            : base(
                ex.Message,
                ex.InnerException
                ) { }
        
        public ConnectionException(AdomdErrorResponseException ex)
            : base(
                ex.Message,
                ex.InnerException
                ) { }

        public ConnectionException(AdomdConnectionException ex)
            : base(
                ex.Message,
                ex.InnerException
                ) { }

        public ConnectionException(OleDbException ex)
            : base(
                ex.Message,
                ex.InnerException
                ) { }

        public ConnectionException(SqlException ex)
            : base(
                ex.Message,
                ex.InnerException
                ) { }
    }
}
