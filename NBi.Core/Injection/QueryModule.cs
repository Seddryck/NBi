using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Injection;

public class QueryModule : NinjectModule
{
    public override void Load()
    {
        Bind<ClientProvider>().ToSelf().InSingletonScope();
        Bind<CommandProvider>().ToSelf().InSingletonScope();
    }
}
