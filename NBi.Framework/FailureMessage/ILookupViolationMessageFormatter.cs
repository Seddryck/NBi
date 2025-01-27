using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Lookup.Violation;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.FailureMessage;

public interface ILookupViolationMessageFormatter
{
    void Generate(IEnumerable<IResultRow> referenceRows, IEnumerable<IResultRow> candidateRows, LookupViolationCollection violations, ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings);
    string RenderReference();
    string RenderCandidate();
    string RenderAnalysis();
    string RenderPredicate();
    string RenderMessage();
}
