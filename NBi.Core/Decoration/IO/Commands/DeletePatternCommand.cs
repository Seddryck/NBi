using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;
using System.Diagnostics;

namespace NBi.Core.Decoration.IO.Commands
{
    class DeletePatternCommand : IDecorationCommand
    {
        private readonly IDeletePatternCommandArgs args;

        public DeletePatternCommand(IDeletePatternCommandArgs args) => this.args = args;

        public void Execute()
        {
            var path = PathExtensions.CombineOrRoot(args.BasePath, args.Path.Execute());
            Execute(path, args.Pattern.Execute());
        }

        internal void Execute(string path, string pattern)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Deleting file from '{path}' when pattern '{pattern}' is matching ...");
            var dir = new DirectoryInfo(path);

            if (!dir.Exists)
                throw new ExternalDependencyNotFoundException(path);

            var files = dir.GetFiles(pattern, SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Deleting file from '{file.FullName}' ...");
                File.Delete(file.FullName);
            }
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"{files.Count()} file{(files.Count()>1 ? "s" : string.Empty)} deleted from '{path}'.");
        }
    }
}
