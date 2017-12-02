using System;
using System.Data.SqlClient;
using System.Linq;
using NBi.Core.DataManipulation.SqlServer;

namespace NBi.Core.DataManipulation
{
    public class DataManipulationFactory
    {
        public IDecorationCommandImplementation Get(IDataManipulationCommand command)
        {

            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.Instantiate(command.ConnectionString);
            IDataManipulationFactory factory = null;

            if (connection is SqlConnection)
                factory = new SqlServerDataManipulationFactory();

            if (factory != null)
                return factory.Get(command, connection);

            throw new ArgumentException();
        }
    }
}
