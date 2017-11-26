using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Scalar.Resolver;

namespace NBi.Core.Query
{
    public class QueryParameter : IQueryParameter
    {
        private readonly IScalarResolver<object> resolver;

        public QueryParameter(string name, string sqlType, IScalarResolver<object> resolver)
        {
            Name = name;
            SqlType = sqlType;
            this.resolver = resolver;
        }

        internal QueryParameter(string name, IScalarResolver<object> resolver)
            : this(name, string.Empty, resolver)
        {}

        internal QueryParameter(string name, object value)
            : this(name, string.Empty, new LiteralScalarResolver<object>(value))
        {}

        public string Name { get;}
        public string SqlType { get; }

        public object GetValue()
        {
            return resolver.Execute();
        }
    }
}
