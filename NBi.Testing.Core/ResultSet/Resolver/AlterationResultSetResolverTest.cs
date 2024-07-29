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

namespace NBi.Core.Testing.ResultSet.Resolver
{
    public class AlterationResultSetResolverTest
    {
        [Test]
        public void Execute_LoaderOnly_LoaderCalled()
        {
            var embeddedResolverMock = new Mock<IResultSetResolver>();
            embeddedResolverMock.Setup(r => r.Execute()).Returns(new DataTableResultSet());

            var resolver = new AlterationResultSetResolver(embeddedResolverMock.Object, new List<IAlteration>());
            resolver.Execute();

            embeddedResolverMock.Verify(l => l.Execute(), Times.Once);
        }

        [Test]
        public void Execute_LoaderOnly_ReturnsLoadedResultSet()
        {
            var rs = new DataTableResultSet();
            rs.Load("a;1");

            var embeddedresolverMock = new Mock<IResultSetResolver>();
            embeddedresolverMock.Setup(r => r.Execute()).Returns(rs);
            
            var resolver = new AlterationResultSetResolver(embeddedresolverMock.Object, new List<IAlteration>());
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
            var rs = new DataTableResultSet();
            rs.Load("a;1");

            var embeddedResolverMock = new Mock<IResultSetResolver>();
            embeddedResolverMock.Setup(r => r.Execute()).Returns(rs);

            var alterMock = new Mock<IAlteration>();
            alterMock.Setup(t => t.Execute(rs)).Returns(new DataTableResultSet());

            var resolver = new AlterationResultSetResolver(embeddedResolverMock.Object, new List<IAlteration>() { alterMock.Object });
            var result = resolver.Execute();

            Assert.That(result, Is.Not.EqualTo(rs));
            embeddedResolverMock.Verify(l => l.Execute(), Times.Once);
            alterMock.Verify(t => t.Execute(rs), Times.Once);
        }

        [Test]
        public void Execute_LoaderAndTwoAlters_SecondAlterCalledWithResultOfFirst()
        {
            var rs = new DataTableResultSet();
            rs.Load("a;1");

            var embeddedresolverMock = new Mock<IResultSetResolver>();
            embeddedresolverMock.Setup(r => r.Execute()).Returns(rs);

            var intermediateResultSet = new DataTableResultSet();
            var alterMock1 = new Mock<IAlteration>();
            alterMock1.Setup(t => t.Execute(rs)).Returns(intermediateResultSet);

            var finalResultSet = new DataTableResultSet();
            var alterMock2 = new Mock<IAlteration>();
            alterMock2.Setup(t => t.Execute(intermediateResultSet)).Returns(finalResultSet);

            var resolver = new AlterationResultSetResolver(embeddedresolverMock.Object, new List<IAlteration>() { alterMock1.Object, alterMock2.Object });
            var result = resolver.Execute();

            Assert.That(intermediateResultSet, Is.Not.SameAs(finalResultSet));
            embeddedresolverMock.Verify(l => l.Execute(), Times.Once);
            alterMock1.Verify(t => t.Execute(rs), Times.Once);
            alterMock2.Verify(t => t.Execute(intermediateResultSet), Times.Once);
        }
    }
}
