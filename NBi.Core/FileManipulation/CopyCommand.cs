using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBi.Core.FileManipulation
{
    public class CopyCommand : IDecorationCommandImplementation
    {
        private string originalFullPath;
        private string destinationFullPath;

        public CopyCommand(ICopyCommand command)
        {
            originalFullPath = command.SourceFullPath;
            destinationFullPath = command.FullPath;
        }

        public void Execute()
        {
            Execute(originalFullPath, destinationFullPath);
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
