﻿using NBi.Extensibility.Decoration.DataEngineering;
using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.DataEngineering
{
    class BatchRunnerProvider
    {
        public IBatchRunnerFactory Instantiate(string version)
        {
            var directory = AssemblyDirectory;
            var filename = $"NBi.Core.SqlServer.dll";
            var filepath = $"{directory}\\{filename}";
            if (!File.Exists(filepath))
                throw new InvalidOperationException($"Can't find the dll for version '{version}' in '{directory}'. NBi was expecting to find a dll named '{filename}'.");

            var assembly = Assembly.LoadFrom(filepath);
            var types = assembly.GetTypes()
                            .Where(m => m.IsClass && m.GetInterface(typeof(IBatchRunnerFactory).Name) != null);

            if (types.Count() == 0)
                throw new InvalidOperationException($"Can't find a class implementing '{typeof(IBatchRunnerFactory).Name}' in '{assembly.FullName}'.");
            if (types.Count() > 1)
                throw new InvalidOperationException($"Found more than one class implementing '{typeof(IBatchRunnerFactory).Name}' in '{assembly.FullName}'.");

            return Activator.CreateInstance(types.ElementAt(0)) as IBatchRunnerFactory;
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
