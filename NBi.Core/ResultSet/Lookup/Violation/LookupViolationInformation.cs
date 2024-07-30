using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup.Violation
{
    public abstract class LookupViolationInformation
    {
        public RowViolationState State { get; private set; }
        public LookupViolationInformation(RowViolationState state) => State = state;
        public abstract void AddCandidateRow(IResultRow row);
        public abstract IEnumerable<IResultRow> Rows { get; }
    }

    public class LookupExistsViolationInformation : LookupViolationInformation
    {
        public ICollection<IResultRow> CandidateRows { get; private set; } = [];

        public override IEnumerable<IResultRow> Rows => CandidateRows;

        public LookupExistsViolationInformation(RowViolationState state) : base(state) { }

        public override void AddCandidateRow(IResultRow row) => CandidateRows.Add(row);
    }

    public class LookupMatchesViolationInformation : LookupViolationInformation
    {
        public ICollection<LookupMatchesViolationComposite> CandidateRows { get; private set; } = [];
        public LookupMatchesViolationInformation(RowViolationState state)
            : base(state) { }
        public override void AddCandidateRow(IResultRow row) 
            => CandidateRows.Add(new LookupMatchesViolationComposite(row, []));

        public override IEnumerable<IResultRow> Rows => CandidateRows.Select(x => x.CandidateRow);
    }
}
