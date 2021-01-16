using Moq;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Transformation;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.ResultSet.Resolver
{
    public class AlterationResultSetResolverTest
    {
        [Test]
        public void Execute_LoaderOnly_LoaderCalled()
        {
            var embeddedresolverMock = new Mock<IResultSetResolver>();
            embeddedresolverMock.Setup(r => r.Execute()).Returns(new NBi.Core.ResultSet.ResultSet());

            var resolver = new AlterationResultSetResolver(embeddedresolverMock.Object, new List<Alter>());
            resolver.Execute();

            embeddedresolverMock.Verify(l => l.Execute(), Times.Once);
        }

        [Test]
        public void Execute_LoaderOnly_ReturnsLoadedResultSet()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var embeddedresolverMock = new Mock<IResultSetResolver>();
            embeddedresolverMock.Setup(r => r.Execute()).Returns(rs);
            
            var resolver = new AlterationResultSetResolver(embeddedresolverMock.Object, new List<Alter>());
            var result = resolver.Execute();

            Assert.That(result, Is.EqualTo(rs));
        }

        private class TransformationProviderMockable : TransformationProvider
        {
            public TransformationProviderMockable()
                : base(new NBi.Core.Injection.ServiceLocator(), null) { }
        }

        [Test]
        public void Execute_LoaderAndTransformer_TransformerCalledWithLoaderResult()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var embeddedresolverMock = new Mock<IResultSetResolver>();
            embeddedresolverMock.Setup(r => r.Execute()).Returns(rs);

            var alterMock = new Mock<Alter>();
            alterMock.Setup(t => t.Invoke(rs)).Returns(new NBi.Core.ResultSet.ResultSet());

            var resolver = new AlterationResultSetResolver(embeddedresolverMock.Object, new List<Alter>() { alterMock.Object });
            var result = resolver.Execute();

            Assert.That(result, Is.Not.EqualTo(rs));
            embeddedresolverMock.Verify(l => l.Execute(), Times.Once);
            alterMock.Verify(t => t.Invoke(rs), Times.Once);
        }

        [Test]
        public void Execute_LoaderAndTwoAlters_SecondAlterCalledWithResultOfFirst()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var embeddedresolverMock = new Mock<IResultSetResolver>();
            embeddedresolverMock.Setup(r => r.Execute()).Returns(rs);

            var intermediateResultSet = new NBi.Core.ResultSet.ResultSet();
            var alterMock1 = new Mock<Alter>();
            alterMock1.Setup(t => t.Invoke(rs)).Returns(intermediateResultSet);

            var finalResultSet = new NBi.Core.ResultSet.ResultSet();
            var alterMock2 = new Mock<Alter>();
            alterMock2.Setup(t => t.Invoke(intermediateResultSet)).Returns(finalResultSet);

            var resolver = new AlterationResultSetResolver(embeddedresolverMock.Object, new List<Alter>() { alterMock1.Object, alterMock2.Object });
            var result = resolver.Execute();

            Assert.That(intermediateResultSet, Is.Not.EqualTo(finalResultSet));
            embeddedresolverMock.Verify(l => l.Execute(), Times.Once);
            alterMock1.Verify(t => t.Invoke(rs), Times.Once);
            alterMock2.Verify(t => t.Invoke(intermediateResultSet), Times.Once);
        }
    }
}
