using Moq;
using NBi.Core.ResultSet;
using NBi.Core.Transformation;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.ResultSet
{
    public class ResultSetServiceTest
    {
        [Test]
        public void Execute_LoaderOnly_LoaderCalled()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var loaderMock = new Mock<IResultSetResolver>();
            loaderMock.Setup(l => l.Execute()).Returns(rs);
            var loader = loaderMock.Object;

            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var service = builder.GetService();
            service.Execute();

            loaderMock.Verify(l => l.Execute(), Times.Once);
        }

        [Test]
        public void Execute_LoaderOnly_ReturnsLoadedResultSet()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var loaderMock = new Mock<IResultSetResolver>();
            loaderMock.Setup(l => l.Execute()).Returns(rs);
            var loader = loaderMock.Object;

            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            var service = builder.GetService();
            var result = service.Execute();

            Assert.That(result, Is.EqualTo(rs));
        }

        public class TransformationProviderMockable : TransformationProvider
        {
            public TransformationProviderMockable()
                : base(new NBi.Core.Injection.ServiceLocator(), null) { }
        }

        [Test]
        public void Execute_LoaderAndTransformer_TransformerCalledWithLoaderResult()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var loaderStub = new Mock<IResultSetResolver>();
            loaderStub.Setup(l => l.Execute()).Returns(rs);
            var loader = loaderStub.Object;

            var transformerMock = new Mock<TransformationProviderMockable>();
            transformerMock.Setup(l => l.Transform(rs));
            var transformer = transformerMock.Object;

            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            if (transformer!=null)
                builder.Setup(transformer.Transform);
            var service = builder.GetService();
            service.Execute();

            transformerMock.Verify(t => t.Transform(rs), Times.Once);
        }

        [Test]
        public void Execute_LoaderAndTwoAlters_SecondAlterCalledWithResultOfFirst()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var loaderStub = new Mock<IResultSetResolver>();
            loaderStub.Setup(l => l.Execute()).Returns(It.IsAny<NBi.Core.ResultSet.ResultSet>());
            var loader = loaderStub.Object;

            var transformer1Stub = new Mock<TransformationProviderMockable>();
            transformer1Stub.Setup(l => l.Transform(It.IsAny<NBi.Core.ResultSet.ResultSet>())).Returns(rs);
            var transformer1 = transformer1Stub.Object;

            var transformer2Mock = new Mock<TransformationProviderMockable>();
            transformer2Mock.Setup(l => l.Transform(It.IsAny<NBi.Core.ResultSet.ResultSet>()));
            var transformer2 = transformer2Mock.Object;

            var builder = new ResultSetServiceBuilder();
            builder.Setup(loader);
            builder.Setup(transformer1.Transform);
            builder.Setup(transformer2.Transform);
            var service = builder.GetService();
            service.Execute();

            transformer2Mock.Verify(t => t.Transform(rs), Times.Once);
        }
    }
}
