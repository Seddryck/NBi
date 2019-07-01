using NBi.Core.Scalar.Resolver;
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
        private IScalarResolver<string> NewIdentification { get; }

        public NewNameRenamingEngine(IColumnIdentifier originalIdentification, IScalarResolver<string> newIdentification)
            => (OriginalIdentification, NewIdentification) = (originalIdentification, newIdentification);

        public ResultSet Execute(ResultSet rs)
        {
            OriginalIdentification.GetColumn(rs.Table).ColumnName = NewIdentification.Execute();
            return rs;
        }
    }
}
