using DubUrl.Adomd.Mapping;
using DubUrl.Mapping;
using DubUrl.OleDb.Mapping;
using DubUrl.Registering;
using DubUrl.Rewriting.Implementation;
using NBi.Extensibility.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Client;

internal class DubUrlClientFactory : IClientFactory
{
    private ProviderFactoriesRegistrator Registrator { get; } = new();
    private SchemeMapperBuilder SchemeMapperBuilder { get; } = new();

    public DubUrlClientFactory()
    {
        //Automatically load the 3 assemblies (DubUrl, DubUrl.OleDb and DubUrl.Adomd)
        var assemblies = new[] 
        { 
            typeof(OdbcRewriter).Assembly,
            typeof(OleDbRewriter).Assembly,
            typeof(PowerBiDesktopDatabase).Assembly 
        };

        var discovery = new BinFolderDiscoverer(assemblies);
        Registrator = new ProviderFactoriesRegistrator(discovery);
        Registrator.Register();

        SchemeMapperBuilder = new SchemeMapperBuilder(assemblies);
        SchemeMapperBuilder.Build();
    }

    public bool CanHandle(string connectionString)
    {
        if (connectionString.IndexOf("://") < 0)
            return false;

        var scheme = connectionString.Split("://")[0];
        return SchemeMapperBuilder.CanHandle(scheme);
    }

    public IClient Instantiate(string connectionString) 
        => new DubUrlClient(connectionString, SchemeMapperBuilder);
}
