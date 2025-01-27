using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Renaming.Strategies.Missing;

public interface IMissingColumnStrategy
{
    void Execute(string originalColumnName, IResultSet rs);
}
