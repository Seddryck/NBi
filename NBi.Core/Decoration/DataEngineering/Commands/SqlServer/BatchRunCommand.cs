using NBi.Core.Query.Client;
using NBi.Core.Decoration.DataEngineering;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.DataEngineering.Commands.SqlServer
{
    class BatchRunCommand : IDecorationCommand
    {
        private readonly SqlBatchRunCommandArgs args;

        public BatchRunCommand(SqlBatchRunCommandArgs args) => this.args = args;

        public void Execute() 
            => Execute(
                PathExtensions.CombineOrRoot(args.BasePath, args.Path.Execute() ?? string.Empty, args.Name.Execute() ?? string.Empty)
                , args.Version.Execute() ?? string.Empty
                , args.ConnectionString
            );

        protected void Execute(string fullPath, string version, string connectionString)
        {
            var provider = new BatchRunnerProvider();
            var factory = provider.Instantiate(version);
            var args = new BatchRunnerArgs() { FullPath = fullPath, ConnectionString = connectionString };
            var runner = factory.Instantiate(args);
            runner.Execute();
        }
    }
}
