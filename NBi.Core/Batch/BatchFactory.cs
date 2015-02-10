using NBi.Core.Batch.SqlServer;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Batch
{
    class BatchFactory
    {
        public IDecorationCommandImplementation Get(IBatchCommand command)
        {

            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.Get(command.ConnectionString);
            IBatchFatory factory = null;

            if (connection is SqlConnection)
                factory = new SqlServerBatchFactory();

            if (factory != null)
                return factory.Get(command, connection);

            throw new ArgumentException();
        }
    }
}
