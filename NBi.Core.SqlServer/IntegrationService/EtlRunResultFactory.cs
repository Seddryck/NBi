using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Etl;
using Microsoft.SqlServer.Management.IntegrationServices;

namespace NBi.Core.SqlServer2014.IntegrationService
{
    class EtlRunResultFactory
    {
        public EtlRunResult Instantiate(Operation.ServerOperationStatus status, IEnumerable<string> messages, DateTimeOffset? startTime, DateTimeOffset? endTime)
        {
            switch (status)
            {
                case Operation.ServerOperationStatus.Failed:
                    return EtlRunResult.Failure(String.Join(Environment.NewLine, messages));
                case Operation.ServerOperationStatus.Success:
                    return EtlRunResult.Success(endTime.Value.Subtract(startTime.Value));
                default:
                    break;
            }
            throw new ArgumentException();
        }
    }
}
