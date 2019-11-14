using NBi.Core.Calculation;
using NBi.Core.Calculation.Grouping;
using NBi.Core.Calculation.Grouping.ColumnBased;
using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predicate.Combination;
using NBi.Core.Calculation.Predication;
using NBi.Core.Calculation.Ranking;
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

namespace NBi.Core.ResultSet.Filtering
{
    public class ResultSetFilterFactory
    {
        private ServiceLocator ServiceLocator { get; }

        public ResultSetFilterFactory(ServiceLocator serviceLocator)
            => (ServiceLocator) = (serviceLocator);

        public IResultSetFilter Instantiate(PredicationArgs predicationArgs, Context context)
        {
            if (predicationArgs.Identifier == null)
                throw new ArgumentException("You must specify an operand for a predication. The operand is the column or alias or expression on which the predicate will be evaluated.");

            var factory = new PredicateFactory();
            var predicate = factory.Instantiate(predicationArgs.Predicate);

            var predicationFactory = new PredicationFactory();
            var predication = predicationFactory.Instantiate(predicate, predicationArgs.Identifier);

            var filter = new PredicationFilter(predication, context);
            return filter;
        }

        public IResultSetFilter Instantiate(CombinationOperator combinationOperator, IEnumerable<PredicationArgs> predicationArgs, Context context)
        {
            var predications = new List<IPredication>();

            var predicateFactory = new PredicateFactory();
            var predicationFactory = new PredicationFactory();

            foreach (var predicationArg in predicationArgs)
            {
                if (predicationArg.Identifier == null)
                    throw new ArgumentException("You must specify an operand for a predicate. The operand is the column or alias or expression on which the predicate will be evaluated.");

                var predicate = predicateFactory.Instantiate(predicationArg.Predicate);
                var localPredication = predicationFactory.Instantiate(predicate, predicationArg.Identifier);
                predications.Add(localPredication);
            }

            var predication = predicationFactory.Instantiate(predications, combinationOperator);
            var filter = new PredicationFilter(predication, context);
            return filter;
        }


        public IResultSetFilter Instantiate(IRankingInfo rankingInfo, IEnumerable<IColumnDefinitionLight> columns)
        {
            var groupingFactory = new GroupByFactory();
            var groupingArgs = new ColumnGroupByArgs(columns, Context.None);
            var grouping = groupingFactory.Instantiate(groupingArgs);

            var rankingFactory = new RankingFactory();
            var ranking = rankingFactory.Instantiate(rankingInfo);

            return new GroupByFilter(ranking, grouping);
        }
    }
}
