using System;
using System.Linq;

namespace NBi.Core
{
    public interface IExecutionResult
    {
        bool IsSuccess { get; }
        bool IsFailure { get; }
        string Message { get; }
        TimeSpan TimeElapsed { get; }
    }
}
