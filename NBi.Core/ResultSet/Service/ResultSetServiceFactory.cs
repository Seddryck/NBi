using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.Xml;

namespace NBi.Core.ResultSet.Service
{
    public class ResultSetServiceFactory
    {
        public IResultSetService Instantiate(object obj, CsvProfile profile)
        {
            if (obj is IList<IRow>)
                return new ListRowResultSetService((IList<IRow>)obj);
            else if (obj is IContent)
                return new ContentResultSetService((IContent)obj);
            else if (obj is IDbCommand)
                return new QueryResultSetService((IDbCommand)obj);
            else if (obj is string)
                return new CsvResultSetService((string)obj, profile);
            else if (obj is XPathEngine)
                return new XPathResultSetService((XPathEngine)obj);
            else if (obj is object[])
                return new ObjectArrayResultSetService((object[])obj);
            throw new ArgumentException();
        }
    }
}
