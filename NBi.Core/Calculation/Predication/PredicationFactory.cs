using NBi.Core.Calculation.Predicate;
using NBi.Core.Evaluate;
using NBi.Core.ResultSet;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Calculation.Predication
{
    public class PredicationFactory
    {
        public IPredication Instantiate(IPredicate predicate, IColumnIdentifier operand)
            => new SinglePredication(predicate, operand);

        public IPredication Instantiate(IEnumerable<IPredication> predications, CombinationOperator combinationOperator)
        {
            switch (combinationOperator)
            {
                case CombinationOperator.Or:
                    return new OrCombinationPredication(predications);
                case CombinationOperator.XOr:
                    return new XOrCombinationPredication(predications);
                case CombinationOperator.And:
                    return new AndCombinationPredication(predications);
                default:
                    throw new ArgumentOutOfRangeException(nameof(combinationOperator));
            }
        }

        private readonly IPredication truePredication = new TruePredication(); 
        public IPredication True
        {
            get => truePredication;
        }
    }
}
