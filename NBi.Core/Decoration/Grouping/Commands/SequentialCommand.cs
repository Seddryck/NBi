using NBi.Extensibility;
using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.Grouping.Commands
{
    class SequentialCommand : IDecorationCommand
    {
        private readonly ISequentialCommandArgs args;

        public SequentialCommand(ISequentialCommandArgs args) => this.args = args;

        public void Execute() => Execute(args.Commands);

        internal void Execute(IEnumerable<IDecorationCommandArgs> listOfArgs)
        {
            var factory = new DecorationFactory();
            foreach (var args in listOfArgs)
                factory.Instantiate(args).Execute();
        }
    }
}
