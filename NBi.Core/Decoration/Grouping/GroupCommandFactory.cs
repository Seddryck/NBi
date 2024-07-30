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
            return args switch
            {
                GroupParallelCommandArgs _ => new ParallelCommand(childrenCommands, args.RunOnce),
                GroupSequentialCommandArgs _ => new SequentialCommand(childrenCommands, args.RunOnce),
                _ => throw new ArgumentOutOfRangeException(),
            };
        }
    }
}
