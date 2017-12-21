using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Core.Query;
using NBi.Xml.Items;
using NUnit.Framework;
using NBi.Core.Scalar.Resolver;
using System.Data.SqlClient;
using Moq;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Testing.Integration.Core.Query
{
    [TestFixture]
    public class CommandBuilderTest
    {
        
        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [TestFixtureTearDown]
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
            var conn = new SqlConnection() { ConnectionString = ConnectionStringReader.GetSqlClient() };
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select * from [Sales].[Customer] where CustomerID=@Param"
                && x.Parameters == new List<QueryParameter>() { new QueryParameter("@Param", "int", new LiteralScalarResolver<object>("2")) }
                );
            var factory = new DbCommandFactory();
            var cmd = factory.Instantiate(conn, query);

            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(0), Is.EqualTo(2));
            Assert.That(dr.Read(), Is.False);
        }

        [Test, Category("Sql")]
        public void Build_OneParameterWithTypeNvarchar50_CorrectResultSet()
        {
            var conn = new SqlConnection() { ConnectionString = ConnectionStringReader.GetSqlClient() };
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select * from [Sales].[SalesTerritory] where Name=@Param"
                && x.Parameters == new List<QueryParameter>() { new QueryParameter("@Param", "nvarchar(50)", new LiteralScalarResolver<object>("Canada")) }
                );
            var factory = new DbCommandFactory();
            var cmd = factory.Instantiate(conn, query);
            
            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(1), Is.EqualTo("Canada"));
            Assert.That(dr.Read(), Is.False);
        }

        [Test, Category("Sql")]
        public void Build_OneParameterWithoutTypeInt_CorrectResultSet()
        {
            var conn = new AdomdConnection() { ConnectionString = ConnectionStringReader.GetSqlClient() };
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select * from [Sales].[Customer] where CustomerID=@Param"
                && x.Parameters == new List<QueryParameter>() { new QueryParameter("@Param", string.Empty, new LiteralScalarResolver<object>(2)) }
                );
            var factory = new DbCommandFactory();
            var cmd = factory.Instantiate(conn, query);

            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(0), Is.EqualTo(2));
            Assert.That(dr.Read(), Is.False);
        }

        [Test, Category("Sql")]
        public void Build_WithUselessParameter_CorrectResultSet()
        {
            var conn = new SqlConnection() { ConnectionString = ConnectionStringReader.GetSqlClient() };
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetSqlClient()
                && x.Statement == "select * from [Sales].[SalesTerritory] where Name=@Param"
                && x.Parameters == new List<QueryParameter>() {
                    new QueryParameter("@Param", "Canada"),
                    new QueryParameter("@UnusedParam", "Useless")
                });
            var factory = new DbCommandFactory();
            var cmd = factory.Instantiate(conn, query);
            
            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(1), Is.EqualTo("Canada"));
            Assert.That(dr.Read(), Is.False);
        }

        [Test]
        [Category("Olap")]
        public void BuildMdx_WithUselessParameter_CorrectResultSet()
        {
            var statement = 
                "select " +
                    "[Measures].[Order Count] on 0, " +
                    "strToMember(@Param) on 1 " +
                "from " +
                    "[Adventure Works]";
            var conn = new SqlConnection() { ConnectionString = ConnectionStringReader.GetAdomd() };
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetAdomd()
                && x.Statement == statement
                && x.Parameters == new List<QueryParameter>() {
                    new QueryParameter("@Param","[Product].[Model Name].[Bike Wash]"),
                    new QueryParameter("UnusedParam", "Useless")
                });
            var factory = new DbCommandFactory();
            var cmd = factory.Instantiate(conn, query);
            
            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(0), Is.EqualTo("Bike Wash"));
            Assert.That(dr.Read(), Is.False);
        }
    }
}
