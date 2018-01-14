using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Command
{
    class AdomdCommandFactory : DbCommandFactory
    {
        public override bool CanHandle(IClient client) => client.UnderlyingSessionType == typeof(AdomdConnection);

        protected override string RenameParameter(string originalName)
        {
            if (originalName.StartsWith("@"))
                return originalName.Substring(1, originalName.Length - 1);
            else
                return originalName;
        }
    }
}

