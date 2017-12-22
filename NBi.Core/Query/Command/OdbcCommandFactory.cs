using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Command
{
    class OdbcCommandFactory : DbCommandFactory
    {
        public override bool CanHandle(ISession session) => session.UnderlyingSessionType == typeof(OdbcConnection);
    }
}

