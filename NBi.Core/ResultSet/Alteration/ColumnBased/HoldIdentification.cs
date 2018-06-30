using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.ColumnBased
{
    class HoldIdentification : RemoveIdentification
    {
        public HoldIdentification(IEnumerable<IColumnIdentifier> identifiers)
            : base(identifiers) { }

        protected override bool IsColumnToRemove(ResultSet resultSet, IEnumerable<DataColumn> identifiedColumns, int index)
            => !base.IsColumnToRemove(resultSet, identifiedColumns, index);
    }
}
