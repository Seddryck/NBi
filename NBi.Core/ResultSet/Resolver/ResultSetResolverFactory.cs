using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;
using NBi.Core.Query.Resolver;
using NBi.Core.Injection;

namespace NBi.Core.ResultSet.Resolver
{
    public class ResultSetResolverFactory
    {
        private CsvProfile profile = CsvProfile.SemiColumnDoubleQuote;
        private readonly ServiceLocator serviceLocator;

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
                case ContentResultSetResolverArgs x: return new ContentResultSetResolver(x);
                case RowsResultSetResolverArgs x: return new RowsResultSetResolver(x);
                case QueryResultSetResolverArgs x: return new QueryResultSetResolver(x, serviceLocator);
                case CsvResultSetResolverArgs x: return new CsvResultSetResolver(x);
                case XPathResultSetResolverArgs x: return new XPathResultSetResolver(x);
                case ObjectsResultSetResolverArgs x: return new ObjectsResultSetResolver(x);
                case SequenceCombinationResultSetResolverArgs x: return new SequenceCombinationResultSetResolver(x);
                default: throw new ArgumentOutOfRangeException($"Type '{args.GetType().Name}' is not expected when building a ResultSet");
            }
        }
    }
}