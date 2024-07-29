using NBi.Core.Query;
using NBi.Core.Query.Resolver;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Variable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Scalar.Resolver
{
    public class FormatScalarResolverArgs : IScalarResolverArgs
    {
        public string Text { get; }
        public Context Context { get; }

        public FormatScalarResolverArgs(string text, Context context)
            => (Text, Context) = (text, context);
    }
}
