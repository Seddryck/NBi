using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Analysis.Process.Agent
{
    class SamoSsasAgent : ICubeProcessor
    {
        private readonly ICubeProcess request;

        public SamoSsasAgent(ICubeProcess request)
        {
            this.request = request;
        }

        public IExecutionResult Run()
        {
            var engine = new SamoSsas.Processor();
            engine.Connect(request.ConnectionString, request.Database);

            var start = DateTime.Now;

            var isSuccess = true;
            if(request.Dimensions.Count()>0)
                isSuccess = engine.ProcessDimensions(request.Dimensions.Select(d => d.Name));
            else if (request.MeasureGroups.Count() > 0)
                isSuccess = engine.ProcessMeasureGroups(request.Cube, request.MeasureGroups.Select(mg => mg.Name));
            else if (request.Partitions.Count() > 0)
                isSuccess = engine.ProcessPartitions(request.Cube, request.Partitions.Select(p => p.Name));
            
            var end = DateTime.Now;

            var result = new ExecutionResult(isSuccess, String.Join("\r\n", engine.Errors), end.Subtract(start));

            return result;

        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        private class ExecutionResult : IExecutionResult
        {
            private bool isSuccess;
            private string errorMessage;
            private TimeSpan timeElapsed;

            public ExecutionResult(bool isSuccess, string errorMessage, TimeSpan timeElapsed)
            {
                this.isSuccess = isSuccess;
                this.errorMessage = errorMessage;
                this.timeElapsed = timeElapsed;
            }

            public bool IsSuccess
            {
                get {return isSuccess;}
            }

            public bool IsFailure
            {
                get { return !isSuccess; }
            }

            public string Message
            {
                get { return errorMessage; }
            }

            public TimeSpan TimeElapsed
            {
                get { return timeElapsed; }
            }
        }
    }
}
