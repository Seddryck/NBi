using Moq;
using NBi.Core.Injection;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using NBi.Extensibility;
using NBi.Testing;

namespace NBi.Core.Testing.Query.Execution;

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
        Assert.That(engine, Is.InstanceOf<SqlExecutionEngine>());
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
        Assert.That(engine, Is.InstanceOf<AdomdExecutionEngine>());
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
        Assert.That(engine, Is.InstanceOf<OdbcExecutionEngine>());
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
        Assert.That(engine, Is.InstanceOf<OleDbExecutionEngine>());
    }

    #region Fake
    public class FakeSession : IClient
    {
        public string ConnectionString => "fake://MyConnectionString";

        public Type UnderlyingSessionType => typeof(object);

        public object CreateNew() => throw new NotImplementedException();
    }

    public class FakeSessionFactory : IClientFactory
    {
        public bool CanHandle(string connectionString) => connectionString.StartsWith("fake://");

        public IClient Instantiate(string connectionString) => new FakeSession();
    }

    public class FakeCommand : ICommand
    {
        public object Implementation => new FakeImplementationCommand();

        public object Client => new FakeSession();

        public object CreateNew() => throw new NotImplementedException();
    }

    public class FakeImplementationCommand
    { }

    public class FakeCommandFactory : ICommandFactory
    {
        public bool CanHandle(IClient session) => session is FakeSession;

        public ICommand Instantiate(IClient session, IQuery query, ITemplateEngine engine) => new FakeCommand();
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
        sessionFactory.RegisterFactories([typeof(FakeSessionFactory)]);

        var commandFactory = localServiceLocator.GetCommandFactory();
        commandFactory.RegisterFactories([typeof(FakeCommandFactory)]);

        var factory = new ExecutionEngineFactory(sessionFactory, commandFactory);
        factory.RegisterEngines([typeof(FakeExecutionEngine)]);

        var engine = factory.Instantiate(query);
        Assert.That(engine, Is.InstanceOf<FakeExecutionEngine>());
    }

    [Test]
    public void Instantiate_FakeConnectionStringExtensions_FakeExecutionEngine()
    {
        var localServiceLocator = new ServiceLocator();
        var setupConfig = localServiceLocator.GetConfiguration();
        var extensions = new Dictionary<Type, IDictionary<string, string>>
        {
            { typeof(FakeSessionFactory), new Dictionary<string, string>() },
            { typeof(FakeCommandFactory), new Dictionary<string, string>() },
            { typeof(FakeExecutionEngine), new Dictionary<string, string>() },
        };
        setupConfig.LoadExtensions(extensions);

        var query = Mock.Of<IQuery>(x => x.ConnectionString == "fake://MyConnectionString");

        var factory = localServiceLocator.GetExecutionEngineFactory();
        var engine = factory.Instantiate(query);
        Assert.That(engine, Is.InstanceOf<FakeExecutionEngine>());
    }
}
