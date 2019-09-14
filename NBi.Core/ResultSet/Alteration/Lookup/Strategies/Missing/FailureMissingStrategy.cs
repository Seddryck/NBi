using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Lookup.Strategies.Missing
{
    public class FailureMissingStrategy : IMissingStrategy
    {
        public void Execute(DataRow row, DataColumn originalColumn, DataColumn newColumn)
            => throw new NBiException($"The value '{row[originalColumn.Ordinal]}' is not available on the reference table.");
    }
}
