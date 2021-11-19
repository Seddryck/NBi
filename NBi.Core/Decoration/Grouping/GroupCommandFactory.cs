using NBi.Core.Decoration.Grouping.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core.Decoration.Grouping
{
    class GroupCommandFactory
    {
        public IGroupCommand Instantiate(IGroupCommandArgs args, IEnumerable<IDecorationCommand> childrenCommands)
        {
            switch (args)
            {
                case IParallelCommandArgs _: return new ParallelCommand(childrenCommands);
                case ISequentialCommandArgs _: return new SequentialCommand(childrenCommands);
                default: throw new ArgumentOutOfRangeException();
            }
        }
    }
}
