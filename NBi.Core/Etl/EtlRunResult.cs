using System;
using System.Collections.Generic;
using System.Linq;

namespace NBi.Core.Etl
{
    public class EtlRunResult : IExecutionResult
    {
        private readonly bool isSuccess;
        public bool IsSuccess { get => isSuccess; }

        public bool IsFailure { get => !IsSuccess; }

        private readonly string message;
        public string Message { get => message; }

        private readonly TimeSpan timeElapsed;
        public TimeSpan TimeElapsed { get => timeElapsed; }

        protected EtlRunResult(bool isSuccess) => this.isSuccess = isSuccess;

        protected EtlRunResult(bool isSuccess, TimeSpan timeElapsed)
        {
            this.isSuccess = isSuccess;
            this.timeElapsed = timeElapsed;
        }

        protected EtlRunResult(bool isSuccess, string message)
        {
            this.isSuccess = isSuccess;
            this.message = message;
        }

        public static EtlRunResult Build(ExecResult result, IPackageEvents events)
        {
            switch (result)
            {
                case ExecResult.Failure:
                    return Failure(events.Message);
                case ExecResult.Success:
                    return Success(events.ExecutionTime);
                default:
                    break;
            }
            throw new ArgumentException();
        }

        public static EtlRunResult Failure(string message) => new EtlRunResult(false, message);

        public static EtlRunResult Success(TimeSpan executionTime) => new EtlRunResult(true, executionTime);
    }
}
