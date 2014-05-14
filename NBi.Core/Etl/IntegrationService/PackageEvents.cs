using System;
using System.Linq;
using Microsoft.SqlServer.Dts.Runtime;

namespace NBi.Core.Etl.IntegrationService
{
    class PackageEvents : DefaultEvents, IPackageEvents
    {
        public string Message { get; set; }
        private DateTime StartTime { get; set; }
        public TimeSpan ExecutionTime { get; set; }

        public override bool OnError(DtsObject source, int errorCode, string subComponent, string description, string helpFile, int helpContext, string idofInterfaceWithError)
        {
            if (string.IsNullOrEmpty(subComponent))
                Message = string.Format("{0}\r\n[{2}] - {1}", Message, description, errorCode);
            else
                Message = string.Format("{0}\r\n[{3}] in {1} - {2}", Message, subComponent, description, errorCode);

            return base.OnError(source, errorCode, subComponent, description, helpFile, helpContext, idofInterfaceWithError);
        }

        public override void OnPreExecute(Executable exec, ref bool fireAgain)
        {
            StartTime = DateTime.Now;
            base.OnPreExecute(exec, ref fireAgain);
        }

        public override void OnPostExecute(Executable exec, ref bool fireAgain)
        {
            ExecutionTime = DateTime.Now.Subtract(StartTime);
            base.OnPreExecute(exec, ref fireAgain);
        }
    }
}
