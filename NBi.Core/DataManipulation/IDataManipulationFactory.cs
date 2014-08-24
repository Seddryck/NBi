using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace NBi.Core.DataManipulation
{
    interface IDataManipulationFactory
    {
        IDecorationCommandImplementation Get(IDataManipulationCommand command, IDbConnection connection);
    }
}
