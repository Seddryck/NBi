using Moq;
using NBi.Core.Injection;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NBi.Extensibility.Query;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Sequence.Resolver;

[TestFixture]
public class QuerySequenceResolverTest
{
    [Test]
    public void Execute_QueryEmbedded_CorrectlyExecuted()
    {
        var queryArgs = new EmbeddedQueryResolverArgs(
                "select * from table"
                , "server=.;initiatl catalog=db;integrated security=true"
                , []
                , []
                , new TimeSpan(0, 0, 30)
        );
        var args = new QuerySequenceResolverArgs(queryArgs);

        var executionEngine = Mock.Of<IExecutionEngine>(x => x.ExecuteList<string>() == new List<string>() { "foo", "bar" });
        var executionEngineFactory = Mock.Of<ExecutionEngineFactory>(x => x.Instantiate(It.IsAny<IQuery>()) == executionEngine);
        var queryResolverFactory = new ServiceLocator().GetQueryResolverFactory();

        var serviceLocator = Mock.Of<ServiceLocator>(
            x => x.GetExecutionEngineFactory() == executionEngineFactory
            && x.GetQueryResolverFactory() == queryResolverFactory
        );

        var resolver = new QuerySequenceResolver<string>(args, serviceLocator);
        var elements = resolver.Execute();
        Assert.That(elements.Count, Is.EqualTo(2));
        Assert.That(elements, Has.Member("foo"));
        Assert.That(elements, Has.Member("bar"));
    }

    [Test]
    public void Execute_QueryEmbedded_CorrectCallsToServiceLocatorMethods()
    {
        var queryArgs = new EmbeddedQueryResolverArgs(
                "select * from table"
                , "server=.;initiatl catalog=db;integrated security=true"
                , []
                , []
                , new TimeSpan(0, 0, 30)
        );
        var args = new QuerySequenceResolverArgs(queryArgs);

        var executionEngine = Mock.Of<IExecutionEngine>(x => x.ExecuteList<string>() == new List<string>() { "foo", "bar" });
        var executionEngineFactory = Mock.Of<ExecutionEngineFactory>(x => x.Instantiate(It.IsAny<IQuery>()) == executionEngine);
        var queryResolverFactory = new ServiceLocator().GetQueryResolverFactory();

        var serviceLocator = Mock.Of<ServiceLocator>(
            x => x.GetExecutionEngineFactory() == executionEngineFactory
            && x.GetQueryResolverFactory() == queryResolverFactory
        );

        var resolver = new QuerySequenceResolver<string>(args, serviceLocator);
        var elements = resolver.Execute();
        Mock.Get(executionEngine).Verify(x => x.ExecuteList<string>(), Times.Once);
        Mock.Get(executionEngineFactory).Verify(x => x.Instantiate(It.IsAny<IQuery>()), Times.Once);
    }
}
