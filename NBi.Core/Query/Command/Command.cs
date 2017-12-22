using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Command
{
    class Command : ICommand
    {
        public object Implementation { get; }

        public Command(object implementation)
        {
            Implementation = implementation;
        }

        
    }
}
