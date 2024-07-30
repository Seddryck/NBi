using NBi.Core.Sequence.Resolver.Loop;
using NBi.Extensibility.Resolving;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver
{
    public class LoopSequenceResolver<T> : ISequenceResolver<T>
    {
        private readonly ILoopStrategy strategy;

        public LoopSequenceResolver(ILoopStrategy args)
        {
            strategy = args;
        }

        IList ISequenceResolver.Execute() => this.Execute();
        object IResolver.Execute() => this.Execute();

        public List<T> Execute()
        {
            var list = new List<T>();
            while (strategy.IsOngoing())
                list.Add((T)(strategy.GetNext() ?? throw new NullReferenceException()));
            return list;
        }
    }
}
