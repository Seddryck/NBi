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

        public void Execute() => Execute(args.Version, args.Etl);

        internal void Execute(string version, IEtl etl)
        {
            var directory = AssemblyDirectory;
            var filename = $"NBi.Core.{version}.dll";
            var filepath = $"{directory}\\{filename}";
            if (!File.Exists(filepath))
                throw new InvalidOperationException($"Can't find the dll for version '{args.Version}' in '{directory}'. NBi was expecting to find a dll named '{filename}'.");

            var assembly = Assembly.LoadFrom(filepath);
            var types = assembly.GetTypes()
                            .Where(m => m.IsClass && m.GetInterface("IEtlRunCommand") != null);
            
            if (types.Count()==0)
                throw new InvalidOperationException($"Can't find a class implementing 'IEtlRunCommand' in '{assembly.FullName}'.");
            if (types.Count() > 1)
                throw new InvalidOperationException($"Found more than one class implementing 'IEtlRunCommand' in '{assembly.FullName}'.");

            var etlRunCommand = Activator.CreateInstance(types.ElementAt(0)) as IEtlRunCommand;
            etlRunCommand.Execute(etl);
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
