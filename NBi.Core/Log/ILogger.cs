using System;
using System.Linq;

namespace NBi.Core.Log
{
    public interface ILogger
    {
        void Write(object message);
        Condition Condition { get; }
        Content Content { get; }
    }
}
