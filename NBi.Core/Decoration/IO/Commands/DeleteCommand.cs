using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;
using System.Diagnostics;

namespace NBi.Core.Decoration.IO.Commands
{
    class DeleteCommand : IDecorationCommand
    {
        private readonly IDeleteCommandArgs args;

        public DeleteCommand(IDeleteCommandArgs args) => this.args = args;

        public void Execute()
            => Execute(PathExtensions.CombineOrRoot(args.BasePath, args.Path.Execute(), args.Name.Execute()));

        internal void Execute(string fullPath)
        {
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceVerbose, $"deleting file '{fullPath}' ...");
            if (!File.Exists(fullPath))
                return;

            File.Delete(fullPath);
            Trace.WriteLineIf(Extensibility.NBiTraceSwitch.TraceInfo, $"File deleted '{fullPath}'.");
        }
    }
}
