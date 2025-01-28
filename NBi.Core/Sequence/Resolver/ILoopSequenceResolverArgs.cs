using NBi.Core.Sequence.Resolver.Loop;
using NBi.Extensibility.Resolving;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Sequence.Resolver;

public interface ILoopSequenceResolverArgs : ISequenceResolverArgs
{ }

public interface ISentinelLoopSequenceResolverArgs : ILoopSequenceResolverArgs
{
    IntervalMode IntervalMode { get; }
}
