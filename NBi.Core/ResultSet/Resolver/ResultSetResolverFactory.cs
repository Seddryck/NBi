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
        private CsvProfile profile = CsvProfile.SemiColumnDoubleQuote;
        private readonly ServiceLocator serviceLocator;
        private IDictionary<string, IVariable> Variables { get; }

        public ResultSetResolverFactory(ServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public void Using(CsvProfile profile)
        {
            if (profile != null)
                this.profile = profile;
        }

        public IResultSetResolver Instantiate(ResultSetResolverArgs args)
        {
            switch (args)
            {
                case IterativeResultSetResolverArgs x: return new IterativeResultSetResolver(x.SequenceResolver, x.VariableName, x.Variables, x.ResultSetResolver);
                case ContentResultSetResolverArgs x: return new ContentResultSetResolver(x);
                case RowsResultSetResolverArgs x: return new RowsResultSetResolver(x);
                case QueryResultSetResolverArgs x: return new QueryResultSetResolver(x, serviceLocator);
                case FlatFileResultSetResolverArgs x: return new FlatFileResultSetResolver(x, serviceLocator);
                case DataSerializationResultSetResolverArgs x: return new DataSerializationResultSetResolver(x);
                case ObjectsResultSetResolverArgs x: return new ObjectsResultSetResolver(x);
                case SequenceCombinationResultSetResolverArgs x: return new SequenceCombinationResultSetResolver(x);
                case EmptyResultSetResolverArgs x: return new EmptyResultSetResolver(x);
                case IfUnavailableResultSetResolverArgs x: return new IfUnavailableResultSetResolver(x);
                default: throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a result-set");
            }
        }
    }
}