using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Core
{
    public class ConnectionException : NBiException
    {
        public ConnectionException(ArgumentException ex, string connectionString)
            : this(
                (Exception)ex,
                connectionString
                ) { }

        public ConnectionException(AdomdErrorResponseException ex, string connectionString)
            : this(
                (Exception)ex,
                connectionString
                ) { }

        public ConnectionException(AdomdConnectionException ex, string connectionString)
            : this(
                (Exception)ex,
                connectionString
                ) { }

        public ConnectionException(OleDbException ex, string connectionString)
            : this(
                (Exception)ex,
                connectionString
                ) { }

        public ConnectionException(SqlException ex, string connectionString)
            : this(
                (Exception)ex,
                connectionString
                ) { }

        protected ConnectionException(Exception ex, string connectionString)
            : base(
                ex.Message + string.Format("\r\nThe connection string used was '{0}'", connectionString),
                ex.InnerException
                ) { }
    }
}
