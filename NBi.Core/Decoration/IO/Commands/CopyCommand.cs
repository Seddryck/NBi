using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;

namespace NBi.Core.Decoration.IO.Commands
{
    class CopyCommand : IDecorationCommand
    {
        private readonly ICopyCommandArgs args;

        public CopyCommand(ICopyCommandArgs args) => this.args = args;

        public void Execute()
        {
            var sourceFullPath = Path.Combine(args.Path.Execute(), args.Name.Execute());
            var destinationFullPath = Path.Combine(args.DestinationPath.Execute(), args.DestinationName.Execute());

            Execute(sourceFullPath, destinationFullPath);
        }

        internal void Execute(string original, string destination)
        {
            if (!File.Exists(original))
                throw new ExternalDependencyNotFoundException(original);

            var destinationFolder = Path.GetDirectoryName(destination);
            if (Directory.Exists(destinationFolder))
                Directory.CreateDirectory(destinationFolder);

            File.Copy(original, destination, true);
        }
    }
}
