using Moq;
using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Session;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Query.Execution
{
    public class ExecutionEngineFactoryTest
    {
        private readonly ServiceLocator serviceLocator = new ServiceLocator();

        [Test]
        public void Instantiate_SqlClient_SqlExecutionEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select 1"
                );

            var factory = serviceLocator.GetExecutionEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<SqlExecutionEngine>(engine);
        }

        [Test]
        public void Instantiate_Adomd_AdomdExecutionEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetAdomd()
                && x.Statement == "select 1 on 0"
                );

            var factory = serviceLocator.GetExecutionEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<AdomdExecutionEngine>(engine);
        }

        [Test]
        public void Instantiate_Odbc_OdbcExecutionEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetOdbcSql()
                && x.Statement == "select 1"
                );

            var factory = serviceLocator.GetExecutionEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<OdbcExecutionEngine>(engine);
        }

        [Test]
        public void Instantiate_OleDb_OleDbExecutionEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetOleDbSql()
                && x.Statement == "select 1"
                );

            var factory = serviceLocator.GetExecutionEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<OleDbExecutionEngine>(engine);
        }

        #region Fake
        public class FakeSession : ISession
        {
            public string ConnectionString => "fake://MyConnectionString";

            public Type UnderlyingSessionType => typeof(object);

            public object CreateNew() => throw new NotImplementedException();
        }

        public class FakeSessionFactory : ISessionFactory
        {
            public bool CanHandle(string connectionString) => connectionString.StartsWith("fake://");

            public ISession Instantiate(string connectionString) => new FakeSession();
        }

        public class FakeCommand : ICommand
        {
            public object Implementation => new FakeImplementationCommand();

            public object Session => new FakeSession();

            public object CreateNew() => throw new NotImplementedException();
        }

        public class FakeImplementationCommand
        { }

        public class FakeCommandFactory : ICommandFactory
        {
            public bool CanHandle(ISession session) => session is FakeSession;

            public ICommand Instantiate(ISession session, IQuery query) => new FakeCommand();
        }

        [SupportedCommandType(typeof(FakeImplementationCommand))]
        private class FakeExecutionEngine : IExecutionEngine
        {
            public FakeExecutionEngine(FakeSession session, object command)
            { }

            public DataSet Execute() => throw new NotImplementedException();
            public IEnumerable<T> ExecuteList<T>() => throw new NotImplementedException();
            public object ExecuteScalar() => throw new NotImplementedException();
        }

        #endregion

        [Test]
        public void Instantiate_FakeConnectionString_FakeExecutionEngine()
        {
            var localServiceLocator = new ServiceLocator();

            var query = Mock.Of<IQuery>(x => x.ConnectionString == "fake://MyConnectionString");

            var sessionFactory = localServiceLocator.GetSessionFactory();
            sessionFactory.RegisterFactories(new[] { typeof(FakeSessionFactory) });

            var commandFactory = localServiceLocator.GetCommandFactory();
            commandFactory.RegisterFactories(new[] { typeof(FakeCommandFactory) });

            var factory = new ExecutionEngineFactory(sessionFactory, commandFactory);
            factory.RegisterEngines(new[] { typeof(FakeExecutionEngine) });

            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<FakeExecutionEngine>(engine);
        }

        [Test]
        public void Instantiate_FakeConnectionStringExtensions_FakeExecutionEngine()
        {
            var localServiceLocator = new ServiceLocator();
            var setupConfig = localServiceLocator.GetConfiguration();
            setupConfig.LoadExtensions(new List<Type>() { typeof(FakeSessionFactory), typeof(FakeCommandFactory), typeof(FakeExecutionEngine) });

            var query = Mock.Of<IQuery>(x => x.ConnectionString == "fake://MyConnectionString");

            var factory = localServiceLocator.GetExecutionEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<FakeExecutionEngine>(engine);
        }
    }
}
