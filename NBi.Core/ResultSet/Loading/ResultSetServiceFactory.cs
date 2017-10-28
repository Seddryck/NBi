using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;

namespace NBi.Core.ResultSet.Loading
{
    public class ResultSetServiceFactory
    {
        public IResultSetLoader Instantiate(object obj, CsvProfile profile)
        {
            if (obj is IList<IRow>)
                return new ListRowResultSetLoader((IList<IRow>)obj);
            else if (obj is IContent)
                return new ContentResultSetLoader((IContent)obj);
            else if (obj is IDbCommand)
                return new QueryResultSetLoader((IDbCommand)obj);
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
