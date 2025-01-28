using System;
using System.Data.OleDb;
using Microsoft.Data.SqlClient;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Extensibility;

namespace NBi.Core;

/// <summary>
/// Class handling all the constructor to build a ConnectionException. This exception is specifically managed by the Runtime to display correct and effective information about the issue.
/// </summary>
public class ConnectionException : NBiException
{
    public ConnectionException(Exception ex, string connectionString)
        : base(
            ex.Message + string.Format("\r\nThe connection string used was '{0}'", connectionString),
            ex.InnerException
            ) { }
}
