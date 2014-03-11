using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;

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

        public static EtlRunResult Build(DTSExecResult result, string message)
        {
            switch (result)
            {
                case DTSExecResult.Failure:
                    return Failure(message);
                case DTSExecResult.Success:
                    return Success();
                default:
                    break;
            }
            throw new ArgumentException();
        }

        public static EtlRunResult Failure(string message)
        {
            return new EtlRunResult(false, message);
        }

        public static EtlRunResult Success()
        {
            return new EtlRunResult(true);
        }
    }
}
