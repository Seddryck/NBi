using NBi.Core.Sequence.Resolver.Loop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver
{
    public class LoopSequenceResolver<T> : ISequenceResolver<T>
    {
        private readonly ILoopStrategy<T> strategy;

        public LoopSequenceResolver(ILoopStrategy<T> args)
        {
            strategy = args;
        }

        public IList<T> Execute()
        {
            var list = new List<T>();
            while (strategy.IsOngoing())
                list.Add(strategy.GetNext());
            return list;
        }
    }
}
