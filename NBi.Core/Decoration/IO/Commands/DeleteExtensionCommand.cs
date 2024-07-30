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
    class DeleteExtensionCommand : IDecorationCommand
    {
        private readonly IoDeleteExtensionCommandArgs args;

        public DeleteExtensionCommand(IoDeleteExtensionCommandArgs args) => this.args = args;

        public void Execute()
        {
            var path = PathExtensions.CombineOrRoot(args.BasePath, args.Path.Execute() ?? string.Empty);
            var extensions = (args.Extension.Execute() ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            Execute(path, extensions);
        }

        internal void Execute(string path, string[] extensions)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Deleting file with extension{(extensions.Length > 1 ? "s" : string.Empty)} '{string.Join("', '", extensions)}' from '{path}' ...");
            var dir = new DirectoryInfo(path);

            if (!dir.Exists)
                throw new ExternalDependencyNotFoundException(path);

            var files = dir.GetFilesByExtensions(extensions);
            var fileCount = files.Count();

            foreach (var file in files)
            {
                Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Deleting file '{file.FullName}' ...");
                File.Delete(file.FullName);
            }
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"{fileCount} file{(fileCount > 1 ? "s" : string.Empty)} deleted from '{path}'.");
        }
    }
}
