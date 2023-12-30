using NBi.Core.Injection;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Resolver;
using NBi.Core.Query.Client;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Scalar.Format;
using NBi.Core.FlatFile;
using NBi.Core.Transformation.Transformer;
using NBi.Core.Transformation;

namespace NBi.Core.Injection
{
    public class ServiceLocator
    {
        private readonly IKernel kernel;
        private readonly ConfigurationModule config;

        public ServiceLocator()
        {
            config = new ConfigurationModule();
            kernel = new StandardKernel(config, new QueryModule());
            kernel.Bind<ServiceLocator>().ToConstant(this).InSingletonScope();
        }

        public virtual ClientProvider GetSessionFactory()
        {
            return kernel.Get<ClientProvider>();
        }

        public virtual CommandProvider GetCommandFactory()
        {
            return kernel.Get<CommandProvider>();
        }

        public virtual ExecutionEngineFactory GetExecutionEngineFactory()
        {
            return kernel.Get<ExecutionEngineFactory>();
        }

        public virtual ResultSetResolverFactory GetResultSetResolverFactory()
        {
            return kernel.Get<ResultSetResolverFactory>();
        }

        public virtual QueryResolverFactory GetQueryResolverFactory()
        {
            return kernel.Get<QueryResolverFactory>();
        }

        public virtual FlatFileReaderFactory GetFlatFileReaderFactory()
        {
            return kernel.Get<FlatFileReaderFactory>();
        }

        public virtual ScalarResolverFactory GetScalarResolverFactory()
        {
            return kernel.Get<ScalarResolverFactory>();
        }

        public Configuration.Configuration GetConfiguration()
        {
            return kernel.Get<Configuration.Configuration>();
        }

        public FormatterFactory GetFormatterFactory()
        {
            return kernel.Get<FormatterFactory>();
        }

        public void Dispose()
        {
            config?.Dispose();
            kernel?.Dispose();
        }
    }
}
