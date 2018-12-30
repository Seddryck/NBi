using NBi.Core.Scalar.Resolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver
{
    public class ListSequenceResolver<T> : ISequenceResolver<T>
    {
        private readonly ListSequenceResolverArgs args;

        public ListSequenceResolver(ListSequenceResolverArgs args)
        {
            this.args = args;
        }

        IList ISequenceResolver.Execute() => this.Execute();

        public List<T> Execute()
        {
            var list = new List<T>();
            foreach (var arg in args.Objects)
                list.Add(new LiteralScalarResolver<T>(arg).Execute());
            return list;
        }
    }
}
