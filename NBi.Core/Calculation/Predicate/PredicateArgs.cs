using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Filtering;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate
{
    public class PredicateArgs
    {
        public virtual ColumnType ColumnType { get; set; }
        public virtual ComparerType ComparerType { get; set; }
        public virtual bool Not { get; set; }
    }

    public class ReferencePredicateArgs : PredicateArgs
    {
        public virtual IResolver Reference { get; set; }
    }

    public class CaseSensitivePredicateArgs : ReferencePredicateArgs
    {
        public virtual StringComparison StringComparison { get; set; }
    }

    public class SecondOperandPredicateArgs : ReferencePredicateArgs
    {
        public virtual object SecondOperand { get; set; }
    }

    public class CultureSensitivePredicateArgs : PredicateArgs
    {
        public virtual string Culture { get; set; }
    }
}
