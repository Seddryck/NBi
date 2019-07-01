using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Renaming
{
    class NewNameRenamingEngine : IRenamingEngine
    {
        private IColumnIdentifier OriginalIdentification { get; }
        private ColumnNameIdentifier NewIdentification { get; }

        public NewNameRenamingEngine(IColumnIdentifier originalIdentification, IColumnIdentifier newIdentification)
            => (OriginalIdentification, NewIdentification) = (originalIdentification, newIdentification as ColumnNameIdentifier);

        public ResultSet Execute(ResultSet rs)
        {
            OriginalIdentification.GetColumn(rs.Table).ColumnName = NewIdentification.Name;
            return rs;
        }
    }
}
