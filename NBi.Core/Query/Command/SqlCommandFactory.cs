using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Command
{
    class SqlCommandFactory : DbCommandFactory
    {
        public override bool CanHandle(ISession session) => session.UnderlyingSessionType == typeof(SqlConnection);

        protected override string RenameParameter(string originalName)
        {
            if (!originalName.StartsWith("@") && char.IsLetter(originalName[0]))
                return "@" + originalName;
            else
                return originalName;
        }
    }
}

