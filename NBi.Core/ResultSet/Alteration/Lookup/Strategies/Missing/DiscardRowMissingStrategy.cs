using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Lookup.Strategies.Missing;

public class DiscardRowMissingStrategy : IMissingStrategy
{
    public void Execute(IResultRow row, IResultColumn originalColumn, IResultColumn newColumn)
        => row.Delete();
}
