using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NBi.Core.Decoration.IO;

namespace NBi.Core.Decoration.IO.Commands
{
    class DeleteCommand : IDecorationCommand
    {
        private readonly IDeleteCommandArgs args;

        public DeleteCommand(IDeleteCommandArgs args) => this.args = args;

        public void Execute()
            => Execute(Path.Combine(args.Path.Execute(), args.Name.Execute()));

        internal void Execute(string fullPath)
        {
            if (!File.Exists(fullPath))
                return;

            File.Delete(fullPath);
        }
    }
}
