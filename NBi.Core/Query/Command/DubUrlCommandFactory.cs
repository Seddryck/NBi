using NBi.Core.Query.Client;
using NBi.Extensibility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Command;

internal class DubUrlCommandFactory : DbCommandFactory
{
    public override bool CanHandle(IClient client)
        => client is DubUrlClient;

    protected override string RenameParameter(string originalName)
        => !originalName.StartsWith("@") && char.IsLetter(originalName[0])
            ? "@" + originalName
            : originalName;
}
