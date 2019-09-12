using NBi.Core.Etl;
using NBi.Extensibility.DataEngineering;
using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NBi.Core.Decoration.DataEngineering
{
    public class EtlRunCommand : IDecorationCommand
    {
        private readonly IEtlRunCommandArgs args;

        public EtlRunCommand(IEtlRunCommandArgs args) => this.args = args;

        public void Execute() => Execute(args.Etl);

        protected void Execute(IEtlArgs args)
        {
            var provider = new EtlRunnerProvider();
            var etl = provider.Instantiate(args);
            etl.Execute(); 
        }
    }
}
