using NBi.Core.Query.Client;
using NBi.Extensibility.DataEngineering;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.DataEngineering.Commands.SqlServer
{
    class BatchRunCommand : IDecorationCommand
    {
        private readonly IBatchRunCommandArgs args;

        public BatchRunCommand(IBatchRunCommandArgs args) => this.args = args;

        public void Execute() => Execute(Path.Combine(args.Path.Execute(), args.Name.Execute()), args.Version.Execute(), args.ConnectionString);

        public void Execute(string fullPath, string version, string connectionString)
        { 
            var directory = AssemblyDirectory;
            var filename = $"NBi.Core.{version}.dll";
            var filepath = $"{directory}\\{filename}";
            if (!File.Exists(filepath))
                throw new InvalidOperationException($"Can't find the dll for version '{version}' in '{directory}'. NBi was expecting to find a dll named '{filename}'.");

            var assembly = Assembly.LoadFrom(filepath);
            var types = assembly.GetTypes()
                            .Where(m => m.IsClass && m.GetInterface("IBatchRunCommand") != null);

            if (types.Count() == 0)
                throw new InvalidOperationException(string.Format("Can't find a class implementing 'IBatchRunCommand' in '{0}'.", assembly.FullName));
            if (types.Count() > 1)
                throw new InvalidOperationException(string.Format("Found more than one class implementing 'IBatchRunCommand' in '{0}'.", assembly.FullName));

            var batchRunCommand = Activator.CreateInstance(types.ElementAt(0)) as IBatchRunCommand;
            batchRunCommand.Execute(fullPath, new SqlConnection(args.ConnectionString));
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
