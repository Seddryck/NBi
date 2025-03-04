﻿using Moq;
using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Extensibility.Query;
using DubUrl.Mapping;

namespace NBi.Core.Testing.Query.Command;

[TestFixture]
public class DubUrlCommandFactoryTest
{
    private const string PROVIDER_NAME = "Microsoft.Data.SqlClient";

    [OneTimeSetUp]
    public void Setup() 
    {
        if (!DbProviderFactories.GetProviderInvariantNames().Any(x => x == PROVIDER_NAME))
            DbProviderFactories.RegisterFactory(PROVIDER_NAME, Microsoft.Data.SqlClient.SqlClientFactory.Instance);
    }

    [Test]
    public void Build_TimeoutSpecified_TimeoutSet()
    {
        var conn = new DubUrlClient("mssql://server/db", new SchemeMapperBuilder());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == "mssql://server/db"
            && x.Statement == "WAITFOR DELAY '00:00:15'"
            && x.Timeout == new TimeSpan(0, 0, 5)
            );

        var factory = new DubUrlCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<SqlCommand>());
        Assert.That(((SqlCommand)cmd.Implementation).CommandTimeout, Is.EqualTo(5));
    }

    [Test]
    public void Build_TimeoutSetToZero_TimeoutSet0Seconds()
    {
        var conn = new DubUrlClient("mssql://server/db", new SchemeMapperBuilder());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == "mssql://server/db"
            && x.Statement == "WAITFOR DELAY '00:00:15'"
            && x.Timeout == new TimeSpan(0, 0, 0)
            );

        var factory = new DubUrlCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<SqlCommand>());
        Assert.That(((SqlCommand)cmd.Implementation).CommandTimeout, Is.EqualTo(0));
    }

    [Test]
    public void Build_TimeoutSetTo30_TimeoutSet30Seconds()
    {
        var conn = new DubUrlClient("mssql://server/db", new SchemeMapperBuilder());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == "mssql://server/db"
            && x.Statement == "WAITFOR DELAY '00:00:15'"
            && x.Timeout == new TimeSpan(0, 0, 30)
            );

        var factory = new DubUrlCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<SqlCommand>());
        Assert.That(((SqlCommand)cmd.Implementation).CommandTimeout, Is.EqualTo(30));
    }

    [Test]
    public void Build_CommandTypeSetToText_CommandTypeSetText()
    {
        var conn = new DubUrlClient("mssql://server/db", new SchemeMapperBuilder());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == "mssql://server/db"
            && x.CommandType == System.Data.CommandType.Text
            );

        var factory = new DubUrlCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<SqlCommand>());
        Assert.That(((SqlCommand)cmd.Implementation).CommandType, Is.EqualTo(System.Data.CommandType.Text));
    }

    [Test]
    public void Build_CommandTypeSetToStoredProcedure_CommandTypeSetStoredProcedure()
    {
        var conn = new DubUrlClient("mssql://server/db", new SchemeMapperBuilder());
        var query = Mock.Of<IQuery>(
            x => x.ConnectionString == "mssql://server/db"
            && x.CommandType == System.Data.CommandType.StoredProcedure
            );

        var factory = new DubUrlCommandFactory();
        var cmd = factory.Instantiate(conn, query, null);
        Assert.That(cmd.Implementation, Is.InstanceOf<SqlCommand>());
        Assert.That(((SqlCommand)cmd.Implementation).CommandType, Is.EqualTo(System.Data.CommandType.StoredProcedure));
    }
}
