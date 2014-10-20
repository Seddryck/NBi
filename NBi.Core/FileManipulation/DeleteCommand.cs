using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace NBi.Core.FileManipulation
{
    public class DeleteCommand : IDecorationCommandImplementation
    {
        private string fullPath;

        public DeleteCommand(IDeleteCommand command)
        {
            fullPath = command.FullPath;
        }

        public void Execute()
        {
            Execute(fullPath);
        }

        internal void Execute(string file)
        {
            if (!File.Exists(file))
                return;

            File.Delete(file);
        }
    }
}
