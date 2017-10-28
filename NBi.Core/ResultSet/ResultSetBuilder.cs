using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NBi.Core.Query;
using NBi.Core.ResultSet.Loading;

namespace NBi.Core.ResultSet
{
    public class ResultSetBuilder : IResultSetBuilder
    {
        private readonly CsvProfile profile;
        public ResultSetBuilder()
            : this(CsvProfile.SemiColumnDoubleQuote)
        {
        }

        public ResultSetBuilder(CsvProfile profile)
        {
            this.profile = profile;
        }

        public virtual ResultSet Build(Object obj)
        {
            var factory = new ResultSetServiceFactory();
            var service = factory.Instantiate(obj, profile);
            return service.Execute();
        }
        
        public class Content : IContent
        {
            public IList<IRow> Rows { get; set; }
            public IList<string> Columns { get; set; }

            public Content (IList<IRow> rows, IList<string> columns)
            {
                Rows = rows;
                Columns = columns;
            }
        }
        
    }
}
