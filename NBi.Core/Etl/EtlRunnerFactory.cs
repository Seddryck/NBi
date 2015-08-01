using System;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NBi.Core.Etl
{
    public class EtlRunnerFactory
    {
        public IEtlRunner Get(IEtl etl)
        {
            var directory = AssemblyDirectory;
            var filename = string.Format("NBi.Core.{0}.dll", etl.Version);
            var filepath = string.Format("{0}\\{1}", directory, filename);
            if (!File.Exists(filepath))
                throw new InvalidOperationException(string.Format("Can't find the dll for version '{0}' in '{1}'. NBi was expecting to find a dll named '{2}'.", etl.Version, directory, filename));

            var assembly = Assembly.LoadFrom(filepath);
            var types = assembly.GetTypes()
                            .Where(m => m.IsClass && m.GetInterface("IEtlRunnerFactory") != null);
            
            if (types.Count()==0)
                throw new InvalidOperationException(string.Format("Can't find a class implementing 'IEtlRunnerFactory' in '{0}'.", assembly.FullName));
            if (types.Count() > 1)
                throw new InvalidOperationException(string.Format("Found more than one class implementing 'IEtlRunnerFactory' in '{0}'.", assembly.FullName));

            var etlRunnerFactory = Activator.CreateInstance(types.ElementAt(0)) as IEtlRunnerFactory;

            var etlRunner = etlRunnerFactory.Get(etl);

            return etlRunner;
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
