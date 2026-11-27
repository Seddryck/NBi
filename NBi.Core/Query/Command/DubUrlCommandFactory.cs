using DubUrl.Querying.Parametrizing;
using Microsoft.AnalysisServices.AdomdClient;
using Microsoft.Data.SqlClient;
using NBi.Core.Query.Client;
using NBi.Extensibility.Query;
using System.Data;
using System.Data.Common;

namespace NBi.Core.Query.Command;

internal class DubUrlCommandFactory : DbCommandFactory
{
    public override bool CanHandle(IClient client) => client is DubUrlClient;

    protected override IDbDataParameter CreateParameter(IClient? client, IDbCommand command, string name, object value)
    {
        var url = ((DubUrlClient)client!).Url;

        // Preserve the parameter names expected by existing SQL Server and Analysis Services queries.
        name = command switch
        {
            SqlCommand when name.Length > 0 && char.IsLetter(name[0]) => "@" + name,
            AdomdCommand when name.StartsWith('@') => name[1..],
            _ => name,
        };

        var parameter = url.Parametrizer.CreateParameter(command, new DubUrlParameterObject(name, value));
        if (parameter is DbParameter inferred)
            inferred.ResetDbType();

        return parameter;
    }
}
