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
using NBi.Core.Query.Command;
using NBi.Core.Query.Client;
using System.Data.Common;
using NBi.Extensibility.Query;

namespace NBi.Testing.Integration.Core.Query.Command
{
    [TestFixture]
    public class AdomdCommandFactoryTest
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
            var conn = new AdomdClient(ConnectionStringReader.GetAdomd());
            var query = Mock.Of<IQuery>(
                x => x.ConnectionString == ConnectionStringReader.GetAdomd()
                && x.Statement == statement
                && x.Parameters == new List<QueryParameter>() {
                    new QueryParameter("@Param","[Product].[Model Name].[Bike Wash]"),
                    new QueryParameter("UnusedParam", "Useless")
                });
            var factory = new CommandProvider();
            var cmd = factory.Instantiate(conn, query).Implementation;
            Assert.That(cmd, Is.InstanceOf<AdomdCommand>());

            (cmd as AdomdCommand).Connection.Open();
            var dr = (cmd as AdomdCommand).ExecuteReader(System.Data.CommandBehavior.CloseConnection);
            
            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(0), Is.EqualTo("Bike Wash"));
            Assert.That(dr.Read(), Is.False);
        }
    }
}
