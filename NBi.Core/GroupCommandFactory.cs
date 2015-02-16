using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    class GroupCommandFactory
    {
        public GroupCommand Get(IGroupCommand command)
        {
            if (command.Parallel)
                return GroupCommand.Parallel(command);
            else
                return GroupCommand.Sequential(command);

            throw new ArgumentException();
        }
    }
}
