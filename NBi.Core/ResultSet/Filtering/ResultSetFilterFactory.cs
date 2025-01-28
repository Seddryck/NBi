using NBi.Core.Calculation;
using NBi.Core.Calculation.Asserting;
using NBi.Core.Calculation.Ranking;
using NBi.Core.Injection;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.ResultSet.Filtering;

public class ResultSetFilterFactory
{
    private ServiceLocator ServiceLocator { get; }

    public ResultSetFilterFactory(ServiceLocator serviceLocator)
        => (ServiceLocator) = (serviceLocator);

    public IResultSetFilter Instantiate(IFilteringArgs filteringArgs, Context context)
    {
        return filteringArgs switch
        {
            PredicationArgs args => InstantiatePredication(args, context),
            RankingGroupByArgs args => InstantiateRanking(args, context),
            UniquenessArgs args => InstantiateUniqueness(args, context),
            _ => throw new ArgumentOutOfRangeException(),
        };
    }

    private IResultSetFilter InstantiatePredication(PredicationArgs predicationArgs, Context context)
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

    private IResultSetFilter InstantiateRanking(RankingGroupByArgs args, Context context)
    {
        var ranking = new RankingFactory().Instantiate(args);
        return new GroupByFilter(ranking, args.GroupBy);
    }

    private IResultSetFilter InstantiateUniqueness(UniquenessArgs args, Context context)
        => new UniquenessFilter(args.GroupBy);

    public IResultSetFilter Instantiate(CombinationOperator combinationOperator, IEnumerable<PredicationArgs> predicationArgs, Context context)
    {
        var predicateFactory = new PredicateFactory();
        var predicate = predicateFactory.Instantiate(predicationArgs.Select(x => x.Predicate).ToArray(), combinationOperator);

        var predicationFactory = new PredicationFactory();
        var predication = predicationFactory.Instantiate(predicate, predicationArgs.First().Identifier);
        
        var filter = new PredicationFilter(predication, context);
        return filter;
    }
}
