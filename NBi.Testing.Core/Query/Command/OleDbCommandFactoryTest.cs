using Moq;
using NBi.Core.Query;
using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using NBi.Testing;
using NUnit.Framework.Internal;
using System.Runtime.InteropServices;

namespace NBi.Core.Testing.Query.Command;

[TestFixture()]
public class OleDbCommandFactoryTest
{
    private const string PROVIDER_NAME = "System.Data.OleDb";

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        if (RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows)
            && !DbProviderFactories.GetProviderInvariantNames().Any(x => x == PROVIDER_NAME))
            DbProviderFactories.RegisterFactory(PROVIDER_NAME, OleDbFactory.Instance);
    }

    [SetUp]
    public void SetUp()
    {
        if (!RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
            Assert.Ignore("OLE DB is not available outside Windows");
    }
#pragma warning disable CA1416

    [Test]
    public void Build_TimeoutSpecified_TimeoutSet()
    {
        var conn = new DbClient(DbProviderFactories.GetFactory(PROVIDER_NAME), typeof(OleDbConnection), ConnectionStringReader.GetOleDbSql());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == ConnectionStringReader.GetOleDbSql()
            && x.Statement == "WAITFOR DELAY '00:00:15'"
            && x.Timeout == new TimeSpan(0, 0, 5)
            );

        var factory = new OleDbCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<OleDbCommand>());
        Assert.That(((OleDbCommand)cmd.Implementation).CommandTimeout, Is.EqualTo(5));
    }

    [Test]
    public void Build_TimeoutSetToZero_TimeoutSet0Seconds()
    {
        var conn = new DbClient(DbProviderFactories.GetFactory(PROVIDER_NAME), typeof(OleDbConnection), ConnectionStringReader.GetOleDbSql());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == ConnectionStringReader.GetOleDbSql()
            && x.Statement == "WAITFOR DELAY '00:00:15'"
            && x.Timeout == new TimeSpan(0, 0, 0)
            );

        var factory = new OleDbCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<OleDbCommand>());
        Assert.That(((OleDbCommand)cmd.Implementation).CommandTimeout, Is.EqualTo(0));
    }

    [Test]
    public void Build_TimeoutSetTo30_TimeoutSet30Seconds()
    {
        var conn = new DbClient(DbProviderFactories.GetFactory(PROVIDER_NAME), typeof(OleDbConnection), ConnectionStringReader.GetOleDbSql());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == ConnectionStringReader.GetOleDbSql()
            && x.Statement == "WAITFOR DELAY '00:00:15'"
            && x.Timeout == new TimeSpan(0, 0, 30)
            );

        var factory = new OleDbCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<OleDbCommand>());
        Assert.That(((OleDbCommand)cmd.Implementation).CommandTimeout, Is.EqualTo(30));
    }

    [Test]
    public void Build_CommandTypeSetToText_CommandTypeSetText()
    {
        var conn = new DbClient(DbProviderFactories.GetFactory(PROVIDER_NAME), typeof(OleDbConnection), ConnectionStringReader.GetOleDbSql());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == ConnectionStringReader.GetOleDbSql()
            && x.CommandType == System.Data.CommandType.Text
            );

        var factory = new OleDbCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<OleDbCommand>());
        Assert.That(((OleDbCommand)cmd.Implementation).CommandType, Is.EqualTo(System.Data.CommandType.Text));
    }

    [Test]
    public void Build_CommandTypeSetToStoredProcedure_CommandTypeSetStoredProcedure()
    {
        var conn = new DbClient(DbProviderFactories.GetFactory(PROVIDER_NAME), typeof(OleDbConnection), ConnectionStringReader.GetOleDbSql());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == ConnectionStringReader.GetOleDbSql()
            && x.CommandType == System.Data.CommandType.StoredProcedure
            );

        var factory = new OleDbCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<OleDbCommand>());
        Assert.That(((OleDbCommand)cmd.Implementation).CommandType, Is.EqualTo(System.Data.CommandType.StoredProcedure));
    }
}
#pragma warning restore CA1416

