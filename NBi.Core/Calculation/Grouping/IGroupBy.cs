using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility;
using NBi.Core.ResultSet;

namespace NBi.Core.Calculation.Grouping;

public interface IGroupBy
{
    IDictionary<KeyCollection, IResultSet> Execute(IResultSet resultSet);
}
