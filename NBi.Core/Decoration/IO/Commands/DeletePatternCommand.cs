using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;
using System.Diagnostics;
using NBi.Extensibility;

namespace NBi.Core.Decoration.IO.Commands;

class DeletePatternCommand : IDecorationCommand
{
    private readonly IoDeletePatternCommandArgs args;

    public DeletePatternCommand(IoDeletePatternCommandArgs args) => this.args = args;

    public void Execute()
    {
        var path = PathExtensions.CombineOrRoot(args.BasePath, args.Path.Execute() ?? string.Empty);
        Execute(path, args.Pattern.Execute() ?? string.Empty);
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
        Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"{files.Length} file{(files.Length > 1 ? "s" : string.Empty)} deleted from '{path}'.");
    }
}
