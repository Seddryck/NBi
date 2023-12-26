using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NBi.Xml.Items;
using NUnit.Framework;
using NBi.Core.Scalar.Resolver;
using System.Data.SqlClient;
using Moq;
using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using System.Data.Common;
using NBi.Extensibility.Query;

namespace NBi.Testing.Integration.Core.Query.Command
{
    [TestFixture]
    public class SqlCommandFactoryTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }
        #endregion


        [Test, Category("Sql")]
        public void Build_OneParameterWithTypeInt_CorrectResultSet()
        {
            var conn = new DbClient(DbProviderFactories.GetFactory("System.Data.SqlClient"), typeof(SqlConnection), ConnectionStringReader.GetSqlClient());
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select * from [Sales].[Customer] where CustomerID=@Param"
                && x.Parameters == new List<QueryParameter>() { new QueryParameter("@Param", "int", new LiteralScalarResolver<object>("2")) }
                );
            var factory = new CommandProvider();
            var cmd = factory.Instantiate(conn, query).Implementation;
            Assert.That(cmd, Is.InstanceOf<SqlCommand>());

            (cmd as SqlCommand).Connection.Open();
            var dr = (cmd as SqlCommand).ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(0), Is.EqualTo(2));
            Assert.That(dr.Read(), Is.False);
        }

        [Test, Category("Sql")]
        public void Build_OneParameterWithTypeNvarchar50_CorrectResultSet()
        {
            var conn = new DbClient(DbProviderFactories.GetFactory("System.Data.SqlClient"), typeof(SqlConnection), ConnectionStringReader.GetSqlClient());
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select * from [Sales].[SalesTerritory] where Name=@Param"
                && x.Parameters == new List<QueryParameter>() { new QueryParameter("@Param", "nvarchar(50)", new LiteralScalarResolver<object>("Canada")) }
                );
            var factory = new CommandProvider();
            var cmd = factory.Instantiate(conn, query).Implementation;
            Assert.That(cmd, Is.InstanceOf<SqlCommand>());

            (cmd as SqlCommand).Connection.Open();
            var dr = (cmd as SqlCommand).ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(1), Is.EqualTo("Canada"));
            Assert.That(dr.Read(), Is.False);
        }

        [Test, Category("Sql")]
        public void Build_OneParameterWithoutTypeInt_CorrectResultSet()
        {
            var conn = new DbClient(DbProviderFactories.GetFactory("System.Data.SqlClient"), typeof(SqlConnection), ConnectionStringReader.GetSqlClient());
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select * from [Sales].[Customer] where CustomerID=@Param"
                && x.Parameters == new List<QueryParameter>() { new QueryParameter("@Param", string.Empty, new LiteralScalarResolver<object>(2)) }
                );
            var factory = new CommandProvider();
            var cmd = factory.Instantiate(conn, query).Implementation;
            Assert.That(cmd, Is.InstanceOf<SqlCommand>());

            (cmd as SqlCommand).Connection.Open();
            var dr = (cmd as SqlCommand).ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(0), Is.EqualTo(2));
            Assert.That(dr.Read(), Is.False);
        }

        [Test, Category("Sql")]
        public void Build_WithUselessParameter_CorrectResultSet()
        {
            var conn = new DbClient(DbProviderFactories.GetFactory("System.Data.SqlClient"), typeof(SqlConnection), ConnectionStringReader.GetSqlClient());
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select * from [Sales].[SalesTerritory] where Name=@Param"
                && x.Parameters == new List<QueryParameter>() {
                    new QueryParameter("@Param", "Canada"),
                    new QueryParameter("@UnusedParam", "Useless")
                });
            var factory = new CommandProvider();
            var cmd = factory.Instantiate(conn, query).Implementation;
            Assert.That(cmd, Is.InstanceOf<SqlCommand>());

            (cmd as SqlCommand).Connection.Open();
            var dr = (cmd as SqlCommand).ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(1), Is.EqualTo("Canada"));
            Assert.That(dr.Read(), Is.False);
        }
    }
}
