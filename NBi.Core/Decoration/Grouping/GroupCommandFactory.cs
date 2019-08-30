using NBi.Core.Decoration.Grouping.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Grouping
{
    class GroupCommandFactory
    {
        public IDecorationCommand Instantiate(IGroupCommandArgs args)
        {
            switch (args)
            {
                case IParallelCommandArgs parallelArgs: return new ParallelCommand(parallelArgs);
                case ISequentialCommandArgs sequentialArgs: return new SequentialCommand(sequentialArgs);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
