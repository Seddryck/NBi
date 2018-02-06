using NBi.Core.ResultSet.Lookup;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public interface IReferenceViolationsMessageFormatter
    {
        void Generate(IEnumerable<DataRow> parentRows, IEnumerable<DataRow> childRows, ReferenceViolations violations);
        string RenderParent();
        string RenderChild();
        string RenderAnalysis();
        string RenderPredicate();
        string RenderMessage();
    }
}
