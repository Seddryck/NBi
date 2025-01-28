using NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Command;

class OdbcCommandFactory : DbCommandFactory
{
    public override bool CanHandle(IClient client) => client.UnderlyingSessionType == typeof(OdbcConnection);
}

