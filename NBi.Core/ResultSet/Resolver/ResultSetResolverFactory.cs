using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;
using NBi.Core.Query.Resolver;

namespace NBi.Core.ResultSet.Resolver
{
    public class ResultSetResolverFactory
    {
        private CsvProfile profile = CsvProfile.SemiColumnDoubleQuote;

        public void Using(CsvProfile profile)
        {
            if (profile != null)
                this.profile = profile;
        }

        public IResultSetResolver Instantiate(object obj)
        {
            if (obj is IList<IRow>)
                return new ListRowResultSetResolver((IList<IRow>)obj);
            else if (obj is IContent)
                return new ContentResultSetResolver((IContent)obj);
            else if (obj is QueryResolverArgs)
                return new QueryResultSetResolver((QueryResolverArgs)obj);
            else if (obj is string)
                return new CsvResultSetResolver((string)obj, profile);
            else if (obj is XPathEngine)
                return new XPathResultSetResolver((XPathEngine)obj);
            else if (obj is object[])
                return new ObjectArrayResultSetResolver((object[])obj);

            throw new ArgumentOutOfRangeException($"Type '{obj.GetType().Name}' is not expected when building a ResultSet");
        }
    }
}
