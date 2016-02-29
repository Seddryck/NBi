using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using System.Data;

namespace NBi.Core
{
    /// <summary>
    /// Class handling all the constructor to build a ConnectionException. This exception is specifically managed by the Runtime to display correct and effective information about the issue.
    /// </summary>
    public class CommandTimeoutException : NBiException
    {
        public CommandTimeoutException(Exception ex, IDbCommand command)
            : base(
                string.Format("The query '{0}' with the connection string '{1}' wasn't finished after '{2}' seconds and has thrown a timeout."
                        , command.CommandText
                        , command.Connection.ConnectionString
                        , command.CommandTimeout)
                , ex.InnerException
                ) { }
    }
}
