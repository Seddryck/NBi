using NBi.Core.Decoration.DataEngineering;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;

namespace NBi.Core.Decoration.DataEngineering;

interface IDataEngineeringFactory
{
    IDecorationCommand Instantiate(IDataEngineeringCommandArgs args, IDbConnection connection);
}
