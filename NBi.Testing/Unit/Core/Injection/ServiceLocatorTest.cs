using NBi.Core.Configuration;
using NBi.Core.Injection;
using NBi.Core.Query.Command;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Resolver;
using NBi.Core.Query.Session;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Injection
{
    public class ServiceLocatorTest
    {
        [Test]
        public void Get_SessionFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetSessionFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<SessionFactory>(obj);
        }

        [Test]
        public void Get_SessionFactory_Singleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetSessionFactory();
            var obj2 = locator.GetSessionFactory();
            Assert.That(obj1, Is.EqualTo(obj2));
        }

        # region Fake
        private class FakeSessionFactory : ISessionFactory
        {
            public bool CanHandle(string connectionString) => true;

            public ISession Instantiate(string connectionString) => throw new NotImplementedException();
        }
        #endregion

        [Test]
        public void Get_SessionFactory_CantAddTwiceTheSameFactory()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetSessionFactory();
            var obj2 = locator.GetSessionFactory();
            obj1.RegisterFactories(new[] { typeof(FakeSessionFactory) });
            Assert.Throws<ArgumentException>(() => obj2.RegisterFactories(new[] { typeof(FakeSessionFactory) }));
        }

        [Test]
        public void Get_CommandFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetCommandFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<CommandFactory>(obj);
        }

        [Test]
        public void Get_CommandFactory_Singleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetCommandFactory();
            var obj2 = locator.GetCommandFactory();
            Assert.That(obj1, Is.EqualTo(obj2));
        }

        [Test]
        public void Get_ResultSetResolverFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetResultSetResolverFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<ResultSetResolverFactory>(obj);
        }

        [Test]
        public void Get_ResultSetResolverFactory_NotSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetResultSetResolverFactory();
            var obj2 = locator.GetResultSetResolverFactory();
            Assert.That(obj1, Is.Not.EqualTo(obj2));
        }

        [Test]
        public void Get_QueryResolverFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetQueryResolverFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<QueryResolverFactory>(obj);
        }

        [Test]
        public void Get_QueryResolverFactory_NotSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetQueryResolverFactory();
            var obj2 = locator.GetQueryResolverFactory();
            Assert.That(obj1, Is.Not.EqualTo(obj2));
        }

        [Test]
        public void Get_ScalarResolverFactory_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetScalarResolverFactory();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<ScalarResolverFactory>(obj);
        }

        [Test]
        public void Get_ScalarResolverFactory_NotSingleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetScalarResolverFactory();
            var obj2 = locator.GetScalarResolverFactory();
            Assert.That(obj1, Is.Not.EqualTo(obj2));
        }

        [Test]
        public void Get_Configuration_Instance()
        {
            var locator = new ServiceLocator();
            var obj = locator.GetConfiguration();
            Assert.That(obj, Is.Not.Null);
            Assert.IsInstanceOf<IConfiguration>(obj);
        }

        [Test]
        public void Get_Configuration_Singleton()
        {
            var locator = new ServiceLocator();
            var obj1 = locator.GetConfiguration();
            var obj2 = locator.GetConfiguration();
            Assert.That(obj1, Is.EqualTo(obj2));
        }
    }
}
