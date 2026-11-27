using DubUrl;
using DubUrl.Adomd.Mapping;
using DubUrl.Mapping;
using DubUrl.Adomd.Wrappers;
using DubUrl.OleDb.Mapping;
using System.Data.Common;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Client;

internal class DubUrlClientFactory : IClientFactory
{
    private static readonly Lazy<ISchemeRegistry> Registry = new(BuildRegistry);

    private static ISchemeRegistry BuildRegistry()
    {
        DbProviderFactories.RegisterFactory("Microsoft.AnalysisServices.AdomdClient", AdomdFactory.Instance);
        DbProviderFactories.RegisterFactory("Microsoft.Data.SqlClient", Microsoft.Data.SqlClient.SqlClientFactory.Instance);
        DbProviderFactories.RegisterFactory("System.Data.Odbc", System.Data.Odbc.OdbcFactory.Instance);
        if (OperatingSystem.IsWindows())
            DbProviderFactories.RegisterFactory("System.Data.OleDb", System.Data.OleDb.OleDbFactory.Instance);

        var catalog = new ProviderCatalogBuilder()
            .AddModule(new AttributeProviderModule(typeof(PowerBiDesktopDatabase).Assembly));
        if (OperatingSystem.IsWindows())
            catalog.AddModule(new AttributeProviderModule(
                typeof(OleDbMapper).Assembly,
                typeof(DubUrl.Provider.Excel.Metadata.MsExcelDatabase).Assembly,
                System.Reflection.Assembly.Load("DubUrl.Provider.Access"),
                System.Reflection.Assembly.Load("DubUrl.Provider.SqlServer"),
                System.Reflection.Assembly.Load("DubUrl.Provider.MySql"),
                System.Reflection.Assembly.Load("DubUrl.Provider.Oracle")));

        return catalog.AddApplicationProviders().Build();
    }

    public bool CanHandle(string connectionString)
    {
        var separator = connectionString.IndexOf("://", StringComparison.Ordinal);
        return separator > 0 && Registry.Value.CanHandle(connectionString[..separator]);
    }

    public IClient Instantiate(string connectionString)
        => new DubUrlClient(CreateUrl(connectionString), connectionString);

    internal static ConnectionUrl CreateUrl(string connectionUrl) => new(connectionUrl, Registry.Value);
}
