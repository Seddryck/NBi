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

        protected override void InsertItem(int index, ColumnMapping item)
        {
            if (!(CheckNameOrIndex(x => x.ChildColumn, item) && CheckNameOrIndex(x => x.ParentColumn, item)))
                throw new NBiException("You can't specify some column-mappings using indexes and others using names.");

            base.InsertItem(index, item);
        }

        private bool CheckNameOrIndex(Func<ColumnMapping, string> target, ColumnMapping item)
        {
            return
                (
                    target(item).StartsWith("#")
                    && this.All(x => target(x).StartsWith("#"))
                )
                ||
                (
                    !target(item).StartsWith("#")
                    && !this.Any(x => target(x).StartsWith("#"))
                );
        }

        private static ColumnMappingCollection @default;
        public static ColumnMappingCollection Default
        {
            get
            {
                @default = @default ?? new ColumnMappingCollection() { new ColumnMapping("#0", "#0", ColumnType.Text) };
                return @default;
            }
        }

    }
}
