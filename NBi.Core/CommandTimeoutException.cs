using System;
using System.Data;
using NBi.Extensibility;

namespace NBi.Core;

/// <summary>
/// Class handling all the constructor to build a ConnectionException. This exception is specifically managed by the Runtime to display correct and effective information about the issue.
/// </summary>
public class CommandTimeoutException : NBiException
{
    public CommandTimeoutException(Exception ex, IDbCommand command)
        : base(
            $@"The query '{command.CommandText}' with the connection string '{command.Connection?.ConnectionString}' " +
            $@"wasn't finished after {command.CommandTimeout} second{(command.CommandTimeout>1 ? "s" : string.Empty)} and has thrown a timeout."
            , ex.InnerException
            )
    { }
}
