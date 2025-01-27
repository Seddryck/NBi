using NBi.Core.Configuration;
using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using Ninject.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Injection;

public class ConfigurationModule : NinjectModule
{
    public override void Load()
    {
        Bind<
            Configuration.Configuration,
            IExtensionsConfiguration,
            IFailureReportProfileConfiguration>()
            .To<Configuration.Configuration>()
            .InSingletonScope();
    }
}
