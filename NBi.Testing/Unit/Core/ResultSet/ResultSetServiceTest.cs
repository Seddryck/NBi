using Moq;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Loading;
using NBi.Core.Transformation;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet
{
    public class ResultSetServiceTest
    {
        [Test]
        public void Execute_LoaderOnly_LoaderCalled()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var loaderMock = new Mock<IResultSetLoader>();
            loaderMock.Setup(l => l.Execute()).Returns(rs);
            var loader = loaderMock.Object;

            var builder = new ResultSetServiceBuilder() { Loader = loader };
            var service = builder.GetService();
            service.Execute();

            loaderMock.Verify(l => l.Execute(), Times.Once);
        }

        [Test]
        public void Execute_LoaderOnly_ReturnsLoadedResultSet()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var loaderMock = new Mock<IResultSetLoader>();
            loaderMock.Setup(l => l.Execute()).Returns(rs);
            var loader = loaderMock.Object;

            var builder = new ResultSetServiceBuilder() { Loader = loader };
            var service = builder.GetService();
            var result = service.Execute();

            Assert.That(result, Is.EqualTo(rs));
        }

        [Test]
        public void Execute_LoaderAndTransformer_TransformerCalledWithLoaderResult()
        {
            var rs = new NBi.Core.ResultSet.ResultSet();
            rs.Load("a;1");

            var loaderStub = new Mock<IResultSetLoader>();
            loaderStub.Setup(l => l.Execute()).Returns(rs);
            var loader = loaderStub.Object;

            var transformerMock = new Mock<TransformationProvider>();
            transformerMock.Setup(l => l.Transform(rs));
            var transformer = transformerMock.Object;

            var builder = new ResultSetServiceBuilder() { Loader = loader };
            builder.AddTransformation(transformer);
            var service = builder.GetService();
            service.Execute();

            transformerMock.Verify(t => t.Transform(rs), Times.Once);
        }
    }
}
