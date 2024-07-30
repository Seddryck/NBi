using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBiRs = NBi.Core.ResultSet;

namespace NBi.Core.ResultSet.Lookup.Violation
{
    public abstract class LookupViolationCollection : Dictionary<KeyCollection, LookupViolationInformation>
    {
        public ColumnMappingCollection KeyMappings { get; set; }
        public ColumnMappingCollection ValueMappings { get; set; }

        public LookupViolationCollection(ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings)
        {
            KeyMappings = keyMappings;
            ValueMappings = valueMappings;
        }

        protected virtual LookupViolationInformation Register(RowViolationState state, NBiRs.KeyCollection key, IResultRow candidateRow)
        {
            if (ContainsKey(key))
            {
                var info = this[key];
                if (info.State != state)
                    throw new ArgumentException("Can't change the state of lookup violation", nameof(state));
                info.AddCandidateRow(candidateRow);
                return info;
            }
            else
            {
                LookupViolationInformation info = state==RowViolationState.Mismatch
                    ? (LookupViolationInformation) new LookupMatchesViolationInformation(state)
                    : new LookupExistsViolationInformation(state);

                info.AddCandidateRow(candidateRow);
                Add(key, info);
                return info;
            }
        }

        //public IEnumerable<IResultRow> GetRows(RowViolationState state)
        //{
        //    if (Count > 0 && !isBuilt)
        //    {
        //        var firstRow = this.ElementAt(0).Value.Rows.ElementAt(0);
        //        foreach (var keyMapping in KeyMappings.Reverse())
        //        {
        //            var column = keyMapping.CandidateColumn.GetColumn(firstRow.Table);
        //            column.ExtendedProperties["NBi::Role"] = ColumnRole.Key;
        //            column.ExtendedProperties["NBi::Lookup"] = keyMapping.ReferenceColumn.Label;
        //        }
        //    }
        //    isBuilt = true;

        //    foreach (var violation in this.Where(x => x.Value.State == state))
        //        foreach (var row in violation.Value.Rows)
        //            yield return row;
        //}
    }

    public class LookupExistsViolationCollection : LookupViolationCollection
    {
        public LookupExistsViolationCollection(ColumnMappingCollection keyMappings)
        : base(keyMappings, []) { }
        public LookupViolationInformation Register(NBiRs.KeyCollection key, IResultRow candidateRow)
            => Register(RowViolationState.Unexpected, key, candidateRow);
    }

    public class LookupMatchesViolationCollection : LookupViolationCollection
    {
        public LookupMatchesViolationCollection(ColumnMappingCollection keyMappings, ColumnMappingCollection valueMappings)
        : base(keyMappings, valueMappings) { }

        public LookupViolationInformation Register(NBiRs.KeyCollection key, IResultRow candidateRow)
            => Register(RowViolationState.Unexpected, key, candidateRow);

        public LookupViolationInformation Register(NBiRs.KeyCollection key, LookupMatchesViolationComposite composite)
        {
            if (ContainsKey(key))
            {
                var info = (this[key] as LookupMatchesViolationInformation) ?? throw new NullReferenceException();
                if (info.State != RowViolationState.Mismatch)
                    throw new ArgumentException("Can't change the state of lookup violation");
                info.CandidateRows.Add(composite);
                return info;
            }
            else
            {
                var info = new LookupMatchesViolationInformation(RowViolationState.Mismatch);
                info.CandidateRows.Add(composite);
                Add(key, info);
                return info;
            }
        }
    }

    public class ReverseLookupExistsViolationCollection : LookupViolationCollection
    {
        public ReverseLookupExistsViolationCollection(ColumnMappingCollection keyMappings)
        : base(keyMappings, []) { }
        public LookupViolationInformation Register(NBiRs.KeyCollection key, IResultRow candidateRow)
            => Register(RowViolationState.Missing, key, candidateRow);
    }
}
