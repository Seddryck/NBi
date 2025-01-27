using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Extensibility;

public interface IExecutionResult
{
    bool IsSuccess { get; }
    bool IsFailure { get; }
    string Message { get; }
    TimeSpan TimeElapsed { get; }
}
