using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NBi.Core
{
    class GroupCommand : IDecorationCommandImplementation
    {
        public static GroupCommand Parallel(IGroupCommand group)
        {
            var cmd = new GroupCommand(group.Commands, true);
            return cmd;
        }

        public static GroupCommand Sequential(IGroupCommand group)
        {
            var cmd = new GroupCommand(group.Commands, false);
            return cmd;
        }

        protected IList<IDecorationCommand> Commands { get; private set; }
        protected Action Action { get; set; }
        public void Execute()
        {
            Action.Invoke();
        }

        protected GroupCommand(IList<IDecorationCommand> commands, bool isParallel)
        {
            if (isParallel)
                Action = Parallel;
            else
                Action = Sequential;
            Commands = commands;
        }

        protected void Parallel()
        {
            var implementations = new List<IDecorationCommandImplementation>();
            foreach (var command in Commands)
            {
                var impl = new DecorationFactory().Instantiate(command);
                implementations.Add(impl);
            }
            System.Threading.Tasks.Parallel.ForEach
                (
                    implementations,
                    i => i.Execute()
                );
        }

        protected void Sequential()
        {
            var implemntations = new List<IDecorationCommandImplementation>();
            foreach (var command in Commands)
            {
                var impl = new DecorationFactory().Instantiate(command);
                impl.Execute();
            }
        }
    }
}
