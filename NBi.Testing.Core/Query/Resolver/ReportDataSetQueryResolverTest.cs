using Moq;
using NBi.Core.Query;
using NBi.Core.Report;
using NBi.Core.Query.Resolver;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using NBi.Testing;

namespace NBi.Core.Testing.Query.Resolver;

[TestFixture]
public class ReportDataSetQueryResolverTest
{
    private ReportDataSetQueryResolverArgs BuildArgs()
    {
        return new ReportDataSetQueryResolverArgs(
            @"C:\", @"Path\", "MyReport", "MyDataSet",
            ConnectionStringReader.GetSqlClient(),
            [new QueryParameter("param", "10")],
            [Mock.Of<IQueryTemplateVariable>(x => x.Name == "operator" && x.Value == "not in" )],
            new TimeSpan(0, 0, 10));
    }

    [Test]
    public void Execute_Args_CommandInstantiated()
    {
        var reportingParserStub = new Mock<IReportingParser>();
        reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<ReportDataSetRequest>())).Returns(
            new ReportingCommand() { Text = "select * from myTable;", CommandType = CommandType.Text });

        var factoryStub = new Mock<ReportingParserFactory>();
        factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

        var resolver = new ReportDataSetQueryResolver(BuildArgs(), factoryStub.Object);
        var cmd = resolver.Execute();

        Assert.That(cmd, Is.Not.Null);
    }

    [Test]
    public void Execute_Args_ConnectionStringAssigned()
    {
        var reportingParserStub = new Mock<IReportingParser>();
        reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<ReportDataSetRequest>())).Returns(
            new ReportingCommand() { Text = "select * from myTable;", CommandType = CommandType.Text });

        var factoryStub = new Mock<ReportingParserFactory>();
        factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

        var resolver = new ReportDataSetQueryResolver(BuildArgs(), factoryStub.Object);
        var query = resolver.Execute();

        Assert.That(query.ConnectionString, Is.Not.Null.And.Not.Empty);
        Assert.That(query.ConnectionString, Is.EqualTo(ConnectionStringReader.GetSqlClient()));
    }

    [Test]
    public void Execute_Args_CommandTextAssigned()
    {
        var reportingParserStub = new Mock<IReportingParser>();
        reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<ReportDataSetRequest>())).Returns(
            new ReportingCommand() { Text = "select * from myTable;", CommandType = CommandType.Text });

        var factoryStub = new Mock<ReportingParserFactory>();
        factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

        var resolver = new ReportDataSetQueryResolver(BuildArgs(), factoryStub.Object);
        var query = resolver.Execute();

        Assert.That(query.Statement, Is.EqualTo("select * from myTable;"));
    }

    [Test]
    public void Execute_Args_CommandTypeAssigned()
    {
        var reportingParserStub = new Mock<IReportingParser>();
        reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<ReportDataSetRequest>())).Returns(
            new ReportingCommand() { Text = "myStoredProcedure", CommandType = CommandType.StoredProcedure });

        var factoryStub = new Mock<ReportingParserFactory>();
        factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

        var resolver = new ReportDataSetQueryResolver(BuildArgs(), factoryStub.Object);
        var query = resolver.Execute();

        Assert.That(query.Statement, Is.EqualTo("myStoredProcedure"));
        Assert.That(query.CommandType, Is.EqualTo(CommandType.StoredProcedure));
    }

    public void Execute_Args_ReportingParserCalledOnce()
    {
        var reportingParserMock = new Mock<IReportingParser>();
        reportingParserMock.Setup(x => x.ExtractCommand(It.IsAny<ReportDataSetRequest>())).Returns(
            new ReportingCommand() { Text = "myStoredProcedure", CommandType = CommandType.StoredProcedure });

        var factoryStub = new Mock<ReportingParserFactory>();
        factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserMock.Object);

        var resolver = new ReportDataSetQueryResolver(BuildArgs(), factoryStub.Object);
        var query = resolver.Execute();

        reportingParserMock.Verify(x => x.ExtractCommand(It.IsAny<ReportDataSetRequest>()), Times.Once);
    }

    [Test]
    public void Execute_Args_ParametersAssigned()
    {
        var reportingParserStub = new Mock<IReportingParser>();
        reportingParserStub.Setup(x => x.ExtractCommand(It.IsAny<ReportDataSetRequest>())).Returns(
            new ReportingCommand() { Text = "select * from myTable;", CommandType = CommandType.Text });

        var factoryStub = new Mock<ReportingParserFactory>();
        factoryStub.Setup(x => x.Instantiate(It.IsAny<string>())).Returns(reportingParserStub.Object);

        var resolver = new ReportDataSetQueryResolver(BuildArgs(), factoryStub.Object);
        var query = resolver.Execute();

        Assert.That(query.Parameters.Count, Is.EqualTo(1));
    }

}
