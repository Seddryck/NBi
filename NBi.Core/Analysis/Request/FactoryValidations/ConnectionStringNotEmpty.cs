using System;
using System.Linq;

namespace NBi.Core.Analysis.Request.FactoryValidations;

internal class ConnectionStringNotEmpty : Validation
{
    private readonly string connectionString;

    internal ConnectionStringNotEmpty(string connectionString)
        : base()
    {
        this.connectionString = connectionString;
    }

    internal override void Apply()
    {
        if (string.IsNullOrEmpty(connectionString))
            GenerateException();
    }

    internal override void GenerateException()
    {
        throw new DiscoveryRequestFactoryException("connectionString");
    }
}
