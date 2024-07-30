using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Lookup
{
    public class ColumnMappingCollection : Collection<ColumnMapping>
    {
        public ColumnMappingCollection()
        { }

        public ColumnMappingCollection(IEnumerable<ColumnMapping> mappings)
            : base(mappings.ToList())
        { }

        protected override void InsertItem(int index, ColumnMapping item)
        {
            if (!(CheckIdentifierConformity(x => x.CandidateColumn, item) && CheckIdentifierConformity(x => x.ReferenceColumn, item)))
                throw new NBiException("You can't specify some column-mappings using a column's position and others using column's name. Use only one of these methods to identify columns.");

            base.InsertItem(index, item);
        }

        private bool CheckIdentifierConformity(Func<ColumnMapping, IColumnIdentifier> target, ColumnMapping item)
            => this.All(x => target(x).GetType() == target(item).GetType());

        private static readonly ColumnMappingCollection @default = [new ColumnMapping(new ColumnOrdinalIdentifier(0), ColumnType.Text)];
        public static ColumnMappingCollection Default
            => @default;
    }
}
