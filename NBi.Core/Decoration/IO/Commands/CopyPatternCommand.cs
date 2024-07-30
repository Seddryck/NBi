using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;
using System.Diagnostics;
using NBi.Extensibility;

namespace NBi.Core.Decoration.IO.Commands
{
    class CopyPatternCommand : IDecorationCommand
    {
        private readonly IoCopyPatternCommandArgs args;

        public CopyPatternCommand(IoCopyPatternCommandArgs args) => this.args = args;

        public void Execute()
        {
            var sourcePath = PathExtensions.CombineOrRoot(args.BasePath, args.SourcePath.Execute() ?? string.Empty);
            var destinationPath = PathExtensions.CombineOrRoot(args.BasePath, args.DestinationPath.Execute() ?? string.Empty);
            Execute(sourcePath, destinationPath, args.Pattern.Execute() ?? string.Empty);
        }

        internal void Execute(string original, string destination, string pattern)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Copying file from '{original}' to '{destination}' when pattern '{pattern}' is matching ...");
            var dir = new DirectoryInfo(original);

            if (!dir.Exists)
                throw new ExternalDependencyNotFoundException(original);

            var destinationFolder = Path.GetDirectoryName(destination);
            if (!Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder ?? string.Empty);

            var files = dir.GetFiles(pattern, SearchOption.TopDirectoryOnly);

            foreach (var file in files)
            {
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Copying file from '{file.FullName}' to '{Path.Combine(destination, file.Name)}' ...");
                File.Copy(file.FullName, Path.Combine(destination, file.Name), true);
            }
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"{files.Length} file{(files.Length > 1 ? "s" : string.Empty)} copied from '{original}' to '{destination}'");
        }
    }
}
