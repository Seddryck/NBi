using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Renaming.Strategies.Missing;

public class FailureMissingColumnStrategy : IMissingColumnStrategy
{
    public void Execute(string originalColumnName, IResultSet dataTable)
    {
        var nameColumns = dataTable.Columns.Select(x => x.Name);
        throw new NBiException($"Impossible to rename the column '{originalColumnName}' because this column doesn't exist in the result-set. List of available columns: '{string.Join("', '", nameColumns)}'");
    }
}
