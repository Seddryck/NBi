using System;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.DataManipulation.SqlServer
{
    class SqlServerDataManipulationFactory : IDataManipulationFactory
    {
        public IDataManipulationImplementation Get(IDataManipulationCommand command)
        {
            if (command is ILoadCommand)
                return new BulkLoadCommand(command as ILoadCommand);
            if (command is IResetCommand)
                return new TruncateCommand(command as IResetCommand);

            throw new ArgumentException();
        }
    }
}
