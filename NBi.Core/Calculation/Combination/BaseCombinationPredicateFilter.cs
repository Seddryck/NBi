using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.Injection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predicate.Combination
{
    abstract class BaseCombinationPredicateFilter : BasePredicateFilter
    {
        protected readonly IEnumerable<Predication> predications;

        public abstract string Description { get; }

        internal BaseCombinationPredicateFilter(ServiceLocator serviceLocator, IEnumerable<IColumnAlias> aliases, IEnumerable<IColumnExpression> expressions, IEnumerable<Predication> predications)
            : base(serviceLocator, aliases, expressions)
        {
            this.predications = predications;
        }

        public override string Describe()
        {
            var sb = new StringBuilder();
            foreach (var predication in predications)
            {
                sb.Append(predication.Operand);
                sb.Append(" ");
                sb.Append(predication.Predicate.ToString());
                sb.Append(" ");
                sb.Append(this.Description);
                sb.Append(" ");
            }
            sb.Remove(sb.Length - this.Description.Length - 2, this.Description.Length + 2);
            sb.Append(".");
            return sb.ToString();
        }
    }
}
