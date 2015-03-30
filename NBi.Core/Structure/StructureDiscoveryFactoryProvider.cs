using Microsoft.AnalysisServices;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Structure.Olap;
using NBi.Core.Structure.Relational;
using NBi.Core.Structure.Tabular;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Structure
{
    public class StructureDiscoveryFactoryProvider
    {
        public const string Olap = "olap";
        public const string Relational = "relational";
        public const string Tabular = "tabular";

        protected IDictionary<string, Type> dico;
        public StructureDiscoveryFactoryProvider()
	    {
            dico = new Dictionary<string, Type>();
            Initialize();
	    }
        
        protected virtual void Initialize()
        {
            dico.Add(Olap, typeof(OlapStructureDiscoveryFactory));
            dico.Add(Relational, typeof(RelationalStructureDiscoveryFactory));
            dico.Add(Tabular, typeof(TabularStructureDiscoveryFactory));
        }

        public IStructureDiscoveryFactory Instantiate(string connectionString)
        {
            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.Get(connectionString);
            var dbType = MapConnectionTypeToDatabaseType(connection);

            if (!dico.Keys.Contains(dbType))
                throw new ArgumentException();

            var factoryType = dico[dbType];
            var ctor = factoryType.GetConstructor(new Type[]{typeof(IDbConnection)});
            var factory = (IStructureDiscoveryFactory)ctor.Invoke(new object[] { connection });

            return factory;
        }

        protected virtual string MapConnectionTypeToDatabaseType(IDbConnection connection)
        {
            if (connection is SqlConnection)
                return Relational;
            if (connection is OleDbConnection)
                return Relational;
            if (connection is OdbcConnection)
                return Relational;
            if (connection is AdomdConnection)
                return InquireFurtherAnalysisService(connection.ConnectionString);
            throw new ArgumentOutOfRangeException();
        }

        protected virtual string InquireFurtherAnalysisService(string connectionString)
        {
            try
            {
                var server = new Server();
                server.Connect(connectionString);
                switch (server.ServerMode)
                {
                    case ServerMode.Default: return Olap;
                    case ServerMode.Multidimensional: return Olap;
                    case ServerMode.SharePoint: return Tabular;
                    case ServerMode.Tabular: return Tabular;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.WriteLineIf(NBiTraceSwitch.TraceWarning,"Can't detect server mode for SSAS, using Olap:" + ex.Message);
                return Olap;
            }
            return Olap;

        }
    }
}
