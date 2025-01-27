using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Asserting;

public class PredicateArgs
{
    public virtual ColumnType ColumnType { get; set; }
    public virtual ComparerType ComparerType { get; set; }
    public virtual bool Not { get; set; }
}

public class ReferencePredicateArgs(IResolver reference) : PredicateArgs
{
    public virtual IResolver Reference { get; set; } = reference;
}

public class CaseSensitivePredicateArgs : ReferencePredicateArgs
{
    public virtual StringComparison StringComparison { get; set; }

    public CaseSensitivePredicateArgs(IResolver reference, StringComparison stringComparison)
        : base(reference)
        => (StringComparison) = (stringComparison);
}

public class SecondOperandPredicateArgs : ReferencePredicateArgs
{
    public virtual object SecondOperand { get; set; }

    public SecondOperandPredicateArgs(IResolver reference, object secondOperand)
        : base(reference)
        => (SecondOperand) = (secondOperand);
}

public class CultureSensitivePredicateArgs(string culture) : PredicateArgs
{
    public virtual string Culture { get; set; } = culture;
}
