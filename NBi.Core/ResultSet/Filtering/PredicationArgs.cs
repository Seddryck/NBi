using NBi.Core.Calculation.Predicate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering
{
    public class PredicationArgs : IFilteringArgs
    {
        public PredicationArgs() { }

        public PredicationArgs(IColumnIdentifier identifier, PredicateArgs predicate)
            => (Identifier, Predicate) = (identifier, predicate);

        public virtual PredicateArgs Predicate { get; set; }
        public virtual IColumnIdentifier Identifier { get; set; }
    }
}
