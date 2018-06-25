using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Mutation.ColumnBased
{
    class FilterIdentification : SkipIdentification
    {
        public FilterIdentification(IEnumerable<IColumnIdentifier> identifiers)
            : base(identifiers) { }

        protected override bool IsColumnToRemove(ResultSet resultSet, IEnumerable<DataColumn> identifiedColumns, int index)
            => !base.IsColumnToRemove(resultSet, identifiedColumns, index);
    }
}
