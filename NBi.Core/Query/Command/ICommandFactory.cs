using NBi.Core.Query.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Query.Command
{
    public interface ICommandFactory
    {
        bool CanHandle(ISession session);
        ICommand Instantiate(ISession session, IQuery query);
    }
}
