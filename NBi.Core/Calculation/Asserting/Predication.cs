using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Variable;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Asserting;

internal class Predication(IPredicate predicate, IColumnIdentifier identifier) : IPredication
{
    public IPredicate InternalPredicate { get; } = predicate;
    public IColumnIdentifier Identifier { get; } = identifier;
    protected internal RowValueExtractor Extractor { get; } = new RowValueExtractor(new ServiceLocator());

    public bool Execute(Context context)
        => InternalPredicate.Execute(Extractor.Execute(context, Identifier));

    public static IPredication AlwaysTrue
        => new Predication(new AlwaysTruePredicate(), new ColumnOrdinalIdentifier(0));

    private class AlwaysTruePredicate : IPredicate
    {
        public bool Execute(object? x) => true;
    }
}
