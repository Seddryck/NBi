using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;
using System.Diagnostics;
using NBi.Extensibility;

namespace NBi.Core.Decoration.IO.Commands;

class CopyCommand : IDecorationCommand
{
    private readonly IoCopyCommandArgs args;

    public CopyCommand(IoCopyCommandArgs args) => this.args = args;

    public void Execute()
    {
        var sourceFullPath = PathExtensions.CombineOrRoot(args.BasePath, args.SourcePath.Execute() ?? string.Empty, args.SourceName.Execute() ?? string.Empty);
        var destinationFullPath = PathExtensions.CombineOrRoot(args.BasePath, args.DestinationPath.Execute() ?? string.Empty, args.DestinationName.Execute() ?? string.Empty);
        Execute(sourceFullPath, destinationFullPath);
    }

    internal void Execute(string original, string destination)
    {
        Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"Copying file from '{original}' to '{destination}' ...");
        if (!File.Exists(original))
            throw new ExternalDependencyNotFoundException(original);

        var destinationFolder = Path.GetDirectoryName(destination);
        if (!Directory.Exists(destinationFolder))
            Directory.CreateDirectory(destinationFolder ?? string.Empty);

        File.Copy(original, destination, true);
        Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"File copied from '{original}' to '{destination}'");
    }
}
