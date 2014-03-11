using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;

namespace NBi.Core.Etl
{
    public class EtlRunResult
    {
        public bool IsSuccess { get; private set; }
        public bool IsFailure
        {
            get
            {
                return !IsSuccess;
            }
        }

        public string Message { get; private set; }

        protected EtlRunResult(bool isSuccess)
        {
            IsSuccess = isSuccess;
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
            return new EtlRunResult(false) {Message=message};
        }

        public static EtlRunResult Success()
        {
            return new EtlRunResult(true);
        }
    }
}
