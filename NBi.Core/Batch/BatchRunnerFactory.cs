using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Batch
{
    class BatchRunnerFactory
    {
        public IDecorationCommandImplementation Get(IBatchCommand command)
        {
            var connectionFactory = new ConnectionFactory();
            var connection = connectionFactory.Get(command.ConnectionString);

            var directory = AssemblyDirectory;
            var filename = $"NBi.Core.{command.Version}.dll";
            var filepath = $"{directory}\\{filename}";
            if (!File.Exists(filepath))
                throw new InvalidOperationException(string.Format("Can't find the dll for version '{0}' in '{1}'. NBi was expecting to find a dll named '{2}'.", "2014", directory, filename));

            var assembly = Assembly.LoadFrom(filepath);
            var types = assembly.GetTypes()
                            .Where(m => m.IsClass && m.GetInterface("IBatchRunnerFatory") != null);

            if (types.Count() == 0)
                throw new InvalidOperationException(string.Format("Can't find a class implementing 'IBatchRunnerFatory' in '{0}'.", assembly.FullName));
            if (types.Count() > 1)
                throw new InvalidOperationException(string.Format("Found more than one class implementing 'IBatchRunnerFatory' in '{0}'.", assembly.FullName));

            var batchFactory = Activator.CreateInstance(types.ElementAt(0)) as IBatchRunnerFatory;

            var batchRunner = batchFactory.Get(command, connection);

            return batchRunner;
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
