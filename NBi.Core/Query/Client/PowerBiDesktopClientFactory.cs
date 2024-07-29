using NBi.Core.PowerBiDesktop;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;

namespace NBi.Core.Query.Client
{
    class PowerBiDesktopClientFactory : IClientFactory
    {
        private readonly PowerBiDesktopConnectionStringBuilder connectionStringBuilder = new PowerBiDesktopConnectionStringBuilder();

        public PowerBiDesktopClientFactory()
        { }

        public PowerBiDesktopClientFactory(PowerBiDesktopConnectionStringBuilder connectionStringBuilder)
        {
            this.connectionStringBuilder = connectionStringBuilder;
        }

        public bool CanHandle(string connectionString)
        {
            return !string.IsNullOrEmpty(ParseConnectionString(connectionString));
        }

        public IClient Instantiate(string connectionString)
        {
            if (!CanHandle(connectionString))
                throw new ArgumentException();

            var csb = new DbConnectionStringBuilder() { ConnectionString = connectionString };
            connectionStringBuilder.Build(csb["pbix"].ToString());
            connectionString = connectionStringBuilder.GetConnectionString();

            return new PowerBiDesktopClient(connectionString);
        }

        private string? ParseConnectionString(string connectionString)
        {
            var providerName = ExtractProviderName(connectionString);
            return providerName;
        }

        private string? ExtractProviderName(string connectionString)
        {
            try
            {
                var csb = new DbConnectionStringBuilder() { ConnectionString = connectionString };

                if (csb.ContainsKey("pbix"))
                    return "Microsoft.AnalysisServices.AdomdClient";
            }
            catch (Exception) { return null; }
            return null;
        }

    }
}

