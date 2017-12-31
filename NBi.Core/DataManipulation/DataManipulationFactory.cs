using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using NBi.Core.DataManipulation.SqlServer;
using NBi.Core.Query.Session;

namespace NBi.Core.DataManipulation
{
    public class DataManipulationFactory
    {
        public IDecorationCommandImplementation Get(IDataManipulationCommand command)
        {

            var sessionFactory = new SessionProvider();
            var connection = sessionFactory.Instantiate(command.ConnectionString).CreateNew() as IDbConnection;
            IDataManipulationFactory factory = null;

            if (connection is SqlConnection)
                factory = new SqlServerDataManipulationFactory();

            if (factory != null)
                return factory.Get(command, connection);

            throw new ArgumentException();
        }
    }
}
