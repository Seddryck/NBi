using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Scalar.Resolver;
using System.Data;
using NBi.Extensibility.Query;

namespace NBi.Core.Query
{
    public class Query : IQuery
    {
        public string Statement { get; }
        public string ConnectionString { get; }
        public TimeSpan Timeout { get; }
        public IEnumerable<IQueryParameter> Parameters { get; }
        public IEnumerable<IQueryTemplateVariable> TemplateTokens { get; }
        public CommandType CommandType { get; }

        public Query(string statement, string connectionString)
            : this(statement, connectionString, new TimeSpan(0, 0, 0))
        { }

        public Query(string statement, string connectionString, TimeSpan timeout)
            : this(statement, connectionString, timeout, [])
        { }

        public Query(string statement, string connectionString, TimeSpan timeout, IEnumerable<IQueryParameter> parameters)
            : this(statement, connectionString, timeout, parameters, [])
        { }

        public Query(string statement, string connectionString, TimeSpan timeout, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> templateTokens)
            : this(statement, connectionString, timeout, parameters, templateTokens, CommandType.Text)
        { }

        public Query(string statement, string connectionString, TimeSpan timeout, IEnumerable<IQueryParameter> parameters, IEnumerable<IQueryTemplateVariable> templateTokens, CommandType commandType)
        {
            Statement = statement;
            ConnectionString = connectionString;
            Timeout = timeout;
            Parameters = parameters;
            TemplateTokens = templateTokens;
            CommandType = commandType;
        }
    }
}
