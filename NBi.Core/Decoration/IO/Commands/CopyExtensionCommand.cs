using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;
using System.Diagnostics;
using NBi.Extensibility;

namespace NBi.Core.Decoration.IO.Commands;

class CopyExtensionCommand : IDecorationCommand
{
    private readonly IoCopyExtensionCommandArgs args;

    public CopyExtensionCommand(IoCopyExtensionCommandArgs args) => this.args = args;

    public void Execute()
    {
        var sourcePath = PathExtensions.CombineOrRoot(args.BasePath, args.SourcePath.Execute() ?? string.Empty);
        var destinationPath = PathExtensions.CombineOrRoot(args.BasePath, args.DestinationPath.Execute() ?? string.Empty);
        var extensions = (args.Extension.Execute() ?? string.Empty).Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
        Execute(sourcePath, destinationPath, extensions);
    }

    internal void Execute(string original, string destination, string[] extensions)
    {
        Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, $"Copying file with extension{(extensions.Length > 1 ? "s" : string.Empty)} '{string.Join("', '", extensions)}' from '{original}' to '{destination}' ...");
        var dir = new DirectoryInfo(original);

        if (!dir.Exists)
            throw new ExternalDependencyNotFoundException(original);

        var destinationFolder = Path.GetDirectoryName(destination);
        if (!Directory.Exists(destinationFolder))
            Directory.CreateDirectory(destinationFolder ?? throw new NullReferenceException());

        var files = dir.GetFilesByExtensions(extensions);

        foreach (var file in files)
        {
            Trace.WriteLineIf(NBiTraceSwitch.TraceVerbose, $"Copying file '{file.FullName}' to '{Path.Combine(destination, file.Name)}' ...");
            File.Copy(file.FullName, Path.Combine(destination, file.Name), true);
        }
        Trace.WriteLineIf(NBiTraceSwitch.TraceInfo, $"{files.Count()} file{(files.Count()>1 ? "s" : string.Empty)} copied from '{original}' to '{destination}'");
    }
}
