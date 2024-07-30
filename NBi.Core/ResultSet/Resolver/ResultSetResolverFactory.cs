using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Injection;
using NBi.Core.FlatFile;
using NBi.Extensibility.Resolving;
using NBi.Core.Variable;

namespace NBi.Core.ResultSet.Resolver
{
    public class ResultSetResolverFactory
    {
        private readonly ServiceLocator serviceLocator;

        public ResultSetResolverFactory(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public IResultSetResolver Instantiate(ResultSetResolverArgs args)
            => args switch
            {
                AlterationResultSetResolverArgs x => new AlterationResultSetResolver(x.Resolver, x.Alterations),
                IterativeResultSetResolverArgs x => new IterativeResultSetResolver(x.SequenceResolver, x.VariableName, x.Variables, x.ResultSetResolver),
                ContentResultSetResolverArgs x => new ContentResultSetResolver(x),
                RowsResultSetResolverArgs x => new RowsResultSetResolver(x),
                QueryResultSetResolverArgs x => new QueryResultSetResolver(x, serviceLocator),
                FlatFileResultSetResolverArgs x => new FlatFileResultSetResolver(x, serviceLocator),
                DataSerializationResultSetResolverArgs x => new DataSerializationResultSetResolver(x),
                ObjectsResultSetResolverArgs x => new ObjectsResultSetResolver(x),
                SequenceCombinationResultSetResolverArgs x => new SequenceCombinationResultSetResolver(x),
                EmptyResultSetResolverArgs x => new EmptyResultSetResolver(x),
                IfUnavailableResultSetResolverArgs x => new IfUnavailableResultSetResolver(x),
                _ => throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a result-set"),
            };
    }
}