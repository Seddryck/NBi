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
            Message = string.Format("{0}: {1}", subComponent, description);
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
