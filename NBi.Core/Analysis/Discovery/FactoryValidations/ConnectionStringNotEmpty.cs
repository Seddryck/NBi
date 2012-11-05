using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NBi.Core.Analysis.Discovery;

namespace NBi.Core.Analysis.Discovery.FactoryValidations
{
    internal class ConnectionStringNotEmpty : FilterNotNull
    {

        internal ConnectionStringNotEmpty(string connectionString)
            : base(connectionString)
        {
        }

        internal override void GenerateException()
        {
            throw new DiscoveryFactoryException("connectionString");
        }
    }
}
