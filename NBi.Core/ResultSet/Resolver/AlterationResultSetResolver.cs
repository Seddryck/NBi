using NBi.Core.ResultSet.Alteration;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Resolver;

class AlterationResultSetResolver : IResultSetResolver
{
    public AlterationResultSetResolver(IResultSetResolver embededResultSetResolver, IEnumerable<IAlteration> alter)
    {
        EmbededResultSetResolver = embededResultSetResolver;
        Alterations = alter;
    }

    private IResultSetResolver EmbededResultSetResolver { get; }
    private IEnumerable<IAlteration> Alterations { get; }

    public IResultSet Execute()
    {
        var resultSet = EmbededResultSetResolver.Execute();
        foreach (var alteration in Alterations)
            resultSet = alteration.Execute(resultSet);

        return resultSet;
    }
}
