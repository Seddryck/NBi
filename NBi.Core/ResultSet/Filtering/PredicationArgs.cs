using NBi.Core.Calculation.Asserting;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering;

public class PredicationArgs(PredicateArgs predicateArgs, IColumnIdentifier identifier) : IFilteringArgs
{
    public virtual PredicateArgs Predicate { get; } = predicateArgs;
    public virtual IColumnIdentifier Identifier { get; } = identifier;
}
