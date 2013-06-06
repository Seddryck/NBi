using System;
using System.Linq;

namespace NBi.Core.Log
{
    public interface ILogger
    {
        void Listen(object engine);
        void Write(bool isFailure);
        Condition Condition { get; }
    }
}
