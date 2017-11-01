using Moq;
using NBi.Core.Query;
using NBi.Core.Report;
using NBi.Core.ResultSet.Resolver.Query;
using NBi.Xml.Items;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Resolver.Query
{
    [TestFixture]
    public class SharedDataSetQueryResolverTest
    {
        private SharedDataSetQueryResolverArgs BuildArgs()
        {
            return new SharedDataSetQueryResolverArgs(
                @"C:\", @"Path\", "MyReport",  
                ConnectionStringReader.GetSqlClient(),
                new List<IQueryParameter>() { new QueryParameterXml() { Name="param", StringValue="10" } },
                new List<IQueryTemplateVariable>() { new QueryTemplateVariableXml() { Name = "operator", Value = "not in" } },
                10);
        }

        [Test]
        public void Execute_Args_CommandInstantiated()
        {
            var reportingParserStub = new Mock<IReportingParser>();
            reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<SharedDatasetRequest>())).Returns(
                new ReportingCommand() { Text="select * from myTable;", CommandType=CommandType.Text });

            var factoryStub = new Mock<ReportingParserFactory>();
            factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

            var resolver = new SharedDataSetQueryResolver(BuildArgs(), factoryStub.Object);
            var cmd = resolver.Execute();

            Assert.That(cmd, Is.Not.Null);
        }

        [Test]
        public void Execute_Args_ConnectionStringAssigned()
        {
            var reportingParserStub = new Mock<IReportingParser>();
            reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<SharedDatasetRequest>())).Returns(
                new ReportingCommand() { Text = "select * from myTable;", CommandType = CommandType.Text });

            var factoryStub = new Mock<ReportingParserFactory>();
            factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

            var resolver = new SharedDataSetQueryResolver(BuildArgs(), factoryStub.Object);
            var cmd = resolver.Execute();

            Assert.That(cmd.Connection.ConnectionString, Is.Not.Null.And.Not.Empty);
            Assert.That(cmd.Connection.ConnectionString, Is.EqualTo(ConnectionStringReader.GetSqlClient()));
        }

        [Test]
        public void Execute_Args_CommandTextAssigned()
        {
            var reportingParserStub = new Mock<IReportingParser>();
            reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<SharedDatasetRequest>())).Returns(
                new ReportingCommand() { Text = "select * from myTable;", CommandType = CommandType.Text });

            var factoryStub = new Mock<ReportingParserFactory>();
            factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

            var resolver = new SharedDataSetQueryResolver(BuildArgs(), factoryStub.Object);
            var cmd = resolver.Execute();

            Assert.That(cmd.CommandText, Is.EqualTo("select * from myTable;"));
        }

        [Test]
        public void Execute_Args_CommandTypeAssigned()
        {
            var reportingParserStub = new Mock<IReportingParser>();
            reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<SharedDatasetRequest>())).Returns(
                new ReportingCommand() { Text = "myStoredProcedure", CommandType = CommandType.StoredProcedure });

            var factoryStub = new Mock<ReportingParserFactory>();
            factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

            var resolver = new SharedDataSetQueryResolver(BuildArgs(), factoryStub.Object);
            var cmd = resolver.Execute();

            Assert.That(cmd.CommandText, Is.EqualTo("myStoredProcedure"));
            Assert.That(cmd.CommandType, Is.EqualTo(CommandType.StoredProcedure));
        }

        [Test]
        public void Execute_Args_ReportingParserCalledOnce()
        {
            var reportingParserMock = new Mock<IReportingParser>();
            reportingParserMock.Setup(x => x.ExtractCommand(It.IsAny<SharedDatasetRequest>())).Returns(
                new ReportingCommand() { Text = "myStoredProcedure", CommandType = CommandType.StoredProcedure });

            var factoryStub = new Mock<ReportingParserFactory>();
            factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserMock.Object);

            var resolver = new SharedDataSetQueryResolver(BuildArgs(), factoryStub.Object);
            var cmd = resolver.Execute();

            reportingParserMock.Verify(x => x.ExtractCommand(It.IsAny<SharedDatasetRequest>()), Times.Once);
        }

        [Test]
        public void Execute_Args_ParametersAssigned()
        {
            var reportingParserStub = new Mock<IReportingParser>();
            reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<SharedDatasetRequest>())).Returns(
                new ReportingCommand() { Text = "select * from myTable;", CommandType = CommandType.Text });

            var factoryStub = new Mock<ReportingParserFactory>();
            factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

            var resolver = new SharedDataSetQueryResolver(BuildArgs(), factoryStub.Object);
            var cmd = resolver.Execute();

            Assert.That(cmd.Parameters, Has.Count.EqualTo(1));
        }
        
    }
}
