using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Uniqueness;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public interface IDataRowsMessageFormatter
    {
        void BuildComparaison(IEnumerable<DataRow> expectedRows, IEnumerable<DataRow> actualRows, ResultResultSet compareResult);
        void BuildDuplication(IEnumerable<DataRow> actualRows, ResultUniqueRows result);
        void BuildFilter(IEnumerable<DataRow> actualRows, IEnumerable<DataRow> filteredRows);
        void BuildCount(IEnumerable<DataRow> actualRows);

        string RenderExpected();
        string RenderActual();
        string RenderAnalysis();
        string RenderMessage();
    }
}
