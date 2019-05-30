using NBi.Core.Decoration.DataEngineering;
using NBi.Extensibility.DataEngineering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Etl
{
    public class EtlRunnerFactory
    {
        public IExecution Instantiate(IEtlExecutable etl)
        {
            var directory = AssemblyDirectory;
            var filename = $"NBi.Core.{etl.Version}.dll";
            var filepath = $"{directory}\\{filename}";
            if (!File.Exists(filepath))
                throw new InvalidOperationException(string.Format("Can't find the dll for version '{0}' in '{1}'. NBi was expecting to find a dll named '{2}'.", etl.Version, directory, filename));

            var assembly = Assembly.LoadFrom(filepath);
            var types = assembly.GetTypes()
                            .Where(m => m.IsClass && m.GetInterface("IEtlRunCommand") != null);

            if (types.Count() == 0)
                throw new InvalidOperationException(string.Format("Can't find a class implementing 'IEtlRunCommand' in '{0}'.", assembly.FullName));
            if (types.Count() > 1)
                throw new InvalidOperationException(string.Format("Found more than one class implementing 'IEtlRunCommand' in '{0}'.", assembly.FullName));

            var etlRun = Activator.CreateInstance(types.ElementAt(0)) as EtlRun;

            return etlRun;
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
