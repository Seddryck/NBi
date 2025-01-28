using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Uniqueness;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage;

public interface IDataRowsMessageFormatter
{
    void BuildComparaison(IEnumerable<IResultRow> expectedRows, IEnumerable<IResultRow> actualRows, ResultResultSet compareResult);
    void BuildDuplication(IEnumerable<IResultRow> actualRows, ResultUniqueRows result);
    void BuildFilter(IEnumerable<IResultRow> actualRows, IEnumerable<IResultRow> filteredRows);
    void BuildCount(IEnumerable<IResultRow> actualRows);

    string RenderExpected();
    string RenderActual();
    string RenderAnalysis();
    string RenderMessage();
}
