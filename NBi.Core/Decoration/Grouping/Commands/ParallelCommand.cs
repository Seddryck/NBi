using NBi.Extensibility.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Decoration.Grouping.Commands
{
    class ParallelCommand : IDecorationCommand
    {
        private readonly IParallelCommandArgs args;

        public ParallelCommand(IParallelCommandArgs args) => this.args = args;

        public void Execute() => Execute(args.Commands);

        internal void Execute(IEnumerable<IDecorationCommandArgs> listOfArgs)
        {
            var factory = new DecorationFactory();
            Parallel.ForEach
                (
                    listOfArgs,
                    x => factory.Instantiate(x).Execute()
                );
        }
    }
}
