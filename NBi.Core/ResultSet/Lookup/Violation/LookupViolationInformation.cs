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
        public abstract void AddCandidateRow(DataRow row);
        public abstract IEnumerable<DataRow> Rows { get; }
    }

    public class LookupExistsViolationInformation : LookupViolationInformation
    {
        public ICollection<DataRow> CandidateRows { get; private set; } = new List<DataRow>();

        public override IEnumerable<DataRow> Rows => CandidateRows;

        public LookupExistsViolationInformation(RowViolationState state) : base(state) { }

        public override void AddCandidateRow(DataRow row) => CandidateRows.Add(row);
    }

    public class LookupMatchesViolationInformation : LookupViolationInformation
    {
        public ICollection<LookupMatchesViolationComposite> CandidateRows { get; private set; } = new List<LookupMatchesViolationComposite>();
        public LookupMatchesViolationInformation(RowViolationState state)
            : base(state) { }
        public override void AddCandidateRow(DataRow row) 
            => CandidateRows.Add(new LookupMatchesViolationComposite(row, new List<LookupMatchesViolationRecord>()));

        public override IEnumerable<DataRow> Rows => CandidateRows.Select(x => x.CandidateRow);
    }
}
