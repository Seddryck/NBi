using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.SqlServer.Management.IntegrationServices;

namespace NBi.Core.Etl
{
    public class EtlRunResult: IExecutionResult
    {
        private readonly bool isSuccess;
        public bool IsSuccess
        {
            get
            {
                return isSuccess;
            }
        }

        public bool IsFailure
        {
            get
            {
                return !IsSuccess;
            }
        }

        private readonly string message;
        public string Message
        {
            get
            {
                return message;
            }
        }

        private readonly TimeSpan timeElapsed;
        public TimeSpan TimeElapsed
        {
            get
            {
                return timeElapsed;
            }
        }

        protected EtlRunResult(bool isSuccess)
        {
            this.isSuccess = isSuccess;
        }

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

        public static EtlRunResult Build(Operation.ServerOperationStatus status, IEnumerable<string> messages, DateTimeOffset? startTime, DateTimeOffset? endTime)
        {
            switch (status)
            {
                case Operation.ServerOperationStatus.Failed:
                    return Failure(String.Join("\r\n",messages));
                case Operation.ServerOperationStatus.Success:
                    return Success(endTime.Value.Subtract(startTime.Value));
                default:
                    break;
            }
            throw new ArgumentException();
        }

        public static EtlRunResult Failure(string message)
        {
            return new EtlRunResult(false, message);
        }

        public static EtlRunResult Success(TimeSpan executionTime)
        {
            return new EtlRunResult(true, executionTime);
        }
    }
}
