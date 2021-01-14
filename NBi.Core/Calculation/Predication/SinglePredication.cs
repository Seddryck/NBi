using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predication
{
    class SinglePredication : IPredication
    {
        public IPredicate Predicate { get; }
        public IColumnIdentifier Operand { get; }
        protected internal RowValueExtractor Extractor { get; } = new RowValueExtractor(new ServiceLocator());

        public SinglePredication(IPredicate predicate, IColumnIdentifier operand)
            => (Predicate, Operand) = (predicate, operand);

        public bool Execute(Context context)
            => Predicate.Execute(Extractor.Execute(context, Operand));

        public virtual string Describe()
        {
            var sb = new StringBuilder();
            sb.Append(Operand.Label);
            sb.Append(" ");
            sb.Append(Predicate.ToString());
            sb.Append(".");
            return sb.ToString();
        }
    }

    class TruePredication : IPredication
    {
        public TruePredication()
        { }

        public bool Execute(Context context)
            => true;

        public string Describe()
            => "Always true.";
    }
}
