using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;
using NBi.Core.ResultSet.Resolver.Query;

namespace NBi.Core.ResultSet.Loading
{
    public class ResultSetLoaderFactory
    {
        private CsvProfile profile = CsvProfile.SemiColumnDoubleQuote;

        public void Using(CsvProfile profile)
        {
            if (profile != null)
                this.profile = profile;
        }

        public IResultSetLoader Instantiate(object obj)
        {
            if (obj is IList<IRow>)
                return new ListRowResultSetLoader((IList<IRow>)obj);
            else if (obj is IContent)
                return new ContentResultSetLoader((IContent)obj);
            else if (obj is QueryResolverArgs)
                return new QueryResultSetLoader((QueryResolverArgs)obj);
            else if (obj is string)
                return new CsvResultSetLoader((string)obj, profile);
            else if (obj is XPathEngine)
                return new XPathResultSetLoader((XPathEngine)obj);
            else if (obj is object[])
                return new ObjectArrayResultSetLoader((object[])obj);

            throw new ArgumentOutOfRangeException($"Type '{obj.GetType().Name}' is not expected when building a ResultSet");
        }
    }
}
