using NBi.Core.Decoration.DataEngineering;
using NBi.Extensibility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Decoration.DataEngineering;

namespace NBi.Core.Etl
{
    public class EtlRunnerProvider
    {
        public IEtlRunnerFactory Instantiate(string version)
        {
            var interfaceName = typeof(IEtlRunnerFactory).Name;
            var directory = AssemblyDirectory;
            var filename = $"NBi.Core.{version}.dll";
            var filepath = $"{directory}\\{filename}";
            if (!File.Exists(filepath))
                throw new InvalidOperationException($"Can't find the dll for version '{version}' in '{directory}'. NBi was expecting to find a dll named '{filename}'.");

            var assembly = Assembly.LoadFrom(filepath);
            var types = assembly.GetTypes()
                            .Where(m => m.IsClass && m.GetInterface(interfaceName) != null);

            if (!types.Any())
                throw new InvalidOperationException($"Can't find a class implementing '{interfaceName}' in '{assembly.FullName}'.");
            if (types.Count() > 1)
                throw new InvalidOperationException($"Found more than one class implementing '{interfaceName}' in '{assembly.FullName}'.");

            return (IEtlRunnerFactory?)Activator.CreateInstance(types.ElementAt(0)!) ?? throw new NullReferenceException();
        }

        private static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().Location;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path) ?? string.Empty;
            }
        }
    }
}
