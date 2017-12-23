using NBi.Core.Query.Command;
using NBi.Core.Query.Session;
using NBi.Core.ResultSet.Resolver;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Injection
{
    public class ResolverModule : NinjectModule
    {
        public override void Load()
        {
            Bind<ServiceLocator>().ToSelf();
            Bind<ResultSetResolverFactory>().ToSelf();
        }
    }
}
