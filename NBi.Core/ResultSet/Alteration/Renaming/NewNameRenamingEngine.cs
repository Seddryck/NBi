using NBi.Core.ResultSet.Alteration.Renaming.Strategies.Missing;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
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
        private IMissingColumnStrategy MissingColumnStrategy { get; }

        protected internal NewNameRenamingEngine(IColumnIdentifier originalIdentification, IScalarResolver<string> newIdentification)
            : this(originalIdentification, newIdentification, new FailureMissingColumnStrategy()) { }

        public NewNameRenamingEngine(IColumnIdentifier originalIdentification, IScalarResolver<string> newIdentification, IMissingColumnStrategy missingColumnStrategy)
            => (OriginalIdentification, NewIdentification, MissingColumnStrategy) = (originalIdentification, newIdentification, missingColumnStrategy);

        public IResultSet Execute(IResultSet rs)
        {
            var originalColumn = OriginalIdentification.GetColumn(rs);

            if (originalColumn == null)
                MissingColumnStrategy.Execute(OriginalIdentification.Label, rs);
            else
                originalColumn.Rename(NewIdentification.Execute() ?? throw new NullReferenceException());
            return rs;
        }
    }
}
