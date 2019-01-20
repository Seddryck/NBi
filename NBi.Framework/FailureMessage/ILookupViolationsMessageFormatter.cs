using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage
{
    public interface ILookupViolationsMessageFormatter
    {
        void Generate(IEnumerable<DataRow> referenceRows, IEnumerable<DataRow> candidateRows, LookupViolationCollection violations, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings);
        string RenderReference();
        string RenderCandidate();
        string RenderAnalysis();
        string RenderPredicate();
        string RenderMessage();
    }
}
