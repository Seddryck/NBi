using NBi.Core.ResultSet.Alteration.Lookup.Strategies.Missing;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.Scalar.Resolver;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Alteration.Lookup
{
    public class LookupReplaceArgs : ILookupArgs
    {
        public ColumnMapping Mapping { get; set; }
        public IResultSetService Reference { get; set; }
        public IColumnIdentifier Replacement { get; set; }
        public IMissingStrategy MissingStrategy { get; set; }

        public LookupReplaceArgs(IResultSetService resultSet, ColumnMapping mapping, IColumnIdentifier replacement)
            : this(resultSet, mapping, replacement, new FailureMissingStrategy()) { }

        public LookupReplaceArgs(IResultSetService resultSet, ColumnMapping mapping, IColumnIdentifier replacement, IMissingStrategy missingStrategy)
            => (Reference, Mapping, Replacement, MissingStrategy) = (resultSet, mapping, replacement, missingStrategy);
    }
}
