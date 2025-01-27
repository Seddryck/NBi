using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup.Violation;

public class LookupMatchesViolationComposite
{
    public IResultRow CandidateRow { get; private set; }
    public ICollection<LookupMatchesViolationRecord> Records { get; private set; }

    public LookupMatchesViolationComposite(IResultRow candidateRow, ICollection<LookupMatchesViolationRecord> records) 
        => (CandidateRow, Records) = (candidateRow, records);
}
