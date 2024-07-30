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

        private static readonly NoneServiceLocator noneServiceLocator = new();
        public static ServiceLocator None => noneServiceLocator;

        public ServiceLocator()
        {
            config = new ConfigurationModule();
            kernel = new StandardKernel(config, new QueryModule());
            kernel.Bind<ServiceLocator>().ToConstant(this).InSingletonScope();
        }

        public virtual ClientProvider GetSessionFactory()
            => kernel.Get<ClientProvider>();

        public virtual CommandProvider GetCommandFactory()
            => kernel.Get<CommandProvider>();

        public virtual ExecutionEngineFactory GetExecutionEngineFactory()
            => kernel.Get<ExecutionEngineFactory>();

        public virtual ResultSetResolverFactory GetResultSetResolverFactory()
            => kernel.Get<ResultSetResolverFactory>();

        public virtual QueryResolverFactory GetQueryResolverFactory()
            => kernel.Get<QueryResolverFactory>();

        public virtual FlatFileReaderFactory GetFlatFileReaderFactory()
            => kernel.Get<FlatFileReaderFactory>();

        public virtual ScalarResolverFactory GetScalarResolverFactory()
            => kernel.Get<ScalarResolverFactory>();

        public virtual Configuration.Configuration GetConfiguration()
            => kernel.Get<Configuration.Configuration>();

        public virtual FormatterFactory GetFormatterFactory()
            => kernel.Get<FormatterFactory>();

        public void Dispose()
        {
            config?.Dispose();
            kernel?.Dispose();
        }

        private class NoneServiceLocator : ServiceLocator
        {
            public override ClientProvider GetSessionFactory() => new ();
            public override CommandProvider GetCommandFactory() => new();
            public override ExecutionEngineFactory GetExecutionEngineFactory() => new();
            public override ResultSetResolverFactory GetResultSetResolverFactory() => new(this);
            public override QueryResolverFactory GetQueryResolverFactory() => new(this);
            public override FlatFileReaderFactory GetFlatFileReaderFactory() => new(Configuration.Configuration.Default);
            public override ScalarResolverFactory GetScalarResolverFactory() => new();
            public override Configuration.Configuration GetConfiguration() => new();
            public override FormatterFactory GetFormatterFactory() => new(this);
        }
    }
}
