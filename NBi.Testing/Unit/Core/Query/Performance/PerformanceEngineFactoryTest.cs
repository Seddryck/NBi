using Moq;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBi.Core.Query.Performance;
using NBi.Core.Query.Session;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Query.Performance
{
    public class PerformanceEngineFactoryTest
    {
        [Test]
        public void Instantiate_SqlClient_SqlPerformanceEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select 1"
                );

            var factory = new PerformanceEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<SqlPerformanceEngine>(engine);
        }

        [Test]
        public void Instantiate_Adomd_AdomdPerformanceEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetAdomd()
                && x.Statement == "select 1 on 0"
                );

            var factory = new PerformanceEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<AdomdPerformanceEngine>(engine);
        }

        [Test]
        public void Instantiate_Odbc_OdbcPerformanceEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetOdbcSql()
                && x.Statement == "select 1"
                );

            var factory = new PerformanceEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<OdbcPerformanceEngine>(engine);
        }

        [Test]
        public void Instantiate_OleDb_OleDbPerformanceEngine()
        {
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetOleDbSql()
                && x.Statement == "select 1"
                );

            var factory = new PerformanceEngineFactory();
            var engine = factory.Instantiate(query);
            Assert.IsInstanceOf<OleDbPerformanceEngine>(engine);
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
        private class FakePerformanceEngine : IPerformanceEngine
        {
            public FakePerformanceEngine(FakeSession session, object command)
            { }

            public void CleanCache() => throw new NotImplementedException();
            
            public PerformanceResult Execute(TimeSpan timeout) => throw new NotImplementedException();

            PerformanceResult IPerformanceEngine.Execute() => throw new NotImplementedException();
        }

        #endregion

        //[Test]
        //public void Instantiate_Object_FakePerformanceEngine()
        //{
        //    var query = Mock.Of<IQuery>(x => x.ConnectionString == "fake://MyConnectionString");

        //    var sessionFactory = new SessionFactory();
        //    sessionFactory.RegisterFactories(new[] { typeof(FakeSessionFactory) });

        //    var commandFactory = new CommandFactory();
        //    commandFactory.RegisterFactories(new[] { typeof(FakeCommandFactory) });

        //    var factory = new PerformanceEngineFactory(sessionFactory, commandFactory);
        //    factory.RegisterEngines(new[] { typeof(FakePerformanceEngine) });

        //    var engine = factory.Instantiate(query);
        //    Assert.IsInstanceOf<FakePerformanceEngine>(engine);
        //}
    }
}
