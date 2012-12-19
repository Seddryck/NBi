using System;
using System.Linq;

namespace NBi.Core.Analysis.Request.FactoryValidations
{
    internal class ConnectionStringNotEmpty : FilterNotNull
    {

        internal ConnectionStringNotEmpty(string connectionString)
            : base(connectionString)
        {
        }

        internal override void GenerateException()
        {
            throw new DiscoveryRequestFactoryException("connectionString");
        }
    }
}
