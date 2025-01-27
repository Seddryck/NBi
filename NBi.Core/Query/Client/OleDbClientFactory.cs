using NBi.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using System.Data;

namespace NBi.Core.Query.Client;

class OleDbClientFactory : DbClientFactory
{
    private const string PROVIDER_NAME = "System.Data.OleDb";

    public OleDbClientFactory() 
        : base()
    { }

    protected override IClient Instantiate(DbProviderFactory factory, string connectionString)
        => new DbClient(factory, typeof(OleDbConnection), connectionString);

    protected override DbProviderFactory? ParseConnectionString(string connectionString)
    {
        var csb = GetConnectionStringBuilder(connectionString);
        if (csb == null)
            return null;

        var providerName = ExtractProviderName(csb, connectionString);
        if (string.IsNullOrEmpty(providerName))
            return null;
        
        var factory = GetDbProviderFactory(PROVIDER_NAME);
        return factory;
    }

    protected override DbProviderFactory? GetDbProviderFactory(string providerName)
    {
        if (OperatingSystem.IsWindows())
        {
            var providers = new List<string>();
            foreach (DataRowView item in DbProviderFactories.GetFactoryClasses().DefaultView)
                providers.Add((string)item[2]);

            if (!providers.Any(x => x == providerName))
                DbProviderFactories.RegisterFactory(providerName, OleDbFactory.Instance);

            return base.GetDbProviderFactory(providerName);
        }
        else
            return null;
    }

    protected virtual string ExtractProviderName(DbConnectionStringBuilder connectionStringBuilder, string connectionString)
    {
        if (connectionStringBuilder.ContainsKey("Provider"))
            return connectionStringBuilder["Provider"].ToString()!;
        return string.Empty;
    }
}
