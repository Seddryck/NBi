using System;
using System.Collections.Generic;
using System.Linq;
using NBi.NUnit.Builder;
using NBi.Xml.Items;
using NUnit.Framework;

namespace NBi.Testing.Integration.NUnit.Builder
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
            var commandBuilder = new CommandBuilder();
            var cmd = commandBuilder.Build(
                ConnectionStringReader.GetSqlClient(),
                "select * from [Sales].[Customer] where CustomerID=@Param",
                
                new List<QueryParameterXml>() 
                {
                    new QueryParameterXml()
                    {
                        Name="@Param",
                        SqlType= "int",
                        StringValue = "2"
                    }
                }
                );

            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(0), Is.EqualTo(2));
            Assert.That(dr.Read(), Is.False);
        }

        [Test, Category("Sql")]
        public void Build_OneParameterWithTypeNvarchar50_CorrectResultSet()
        {
            var commandBuilder = new CommandBuilder();
            var cmd = commandBuilder.Build(
                ConnectionStringReader.GetSqlClient(),
                "select * from [Sales].[SalesTerritory] where Name=@Param",

                new List<QueryParameterXml>() 
                {
                    new QueryParameterXml()
                    {
                        Name="@Param",
                        SqlType= "nvarchar(50)",
                        StringValue = "Canada"
                    }
                }
                );

            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(1), Is.EqualTo("Canada"));
            Assert.That(dr.Read(), Is.False);
        }

        [Test, Category("Sql")]
        public void Build_OneParameterWithoutTypeInt_CorrectResultSet()
        {
            var commandBuilder = new CommandBuilder();
            var cmd = commandBuilder.Build(
                ConnectionStringReader.GetSqlClient(),
                "select * from [Sales].[Customer] where CustomerID=@Param",
                
                new List<QueryParameterXml>() 
                {
                    new QueryParameterXml()
                    {
                        Name="@Param",
                        StringValue = "2"
                    }
                }
                );

            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(0), Is.EqualTo(2));
            Assert.That(dr.Read(), Is.False);
        }

        [Test, Category("Sql")]
        public void Build_WithUselessParameter_CorrectResultSet()
        {
            var commandBuilder = new CommandBuilder();
            var cmd = commandBuilder.Build(
                ConnectionStringReader.GetSqlClient(),
                "select * from [Sales].[SalesTerritory] where Name=@Param",

                new List<QueryParameterXml>() 
                {
                    new QueryParameterXml()
                    {
                        Name="@Param",
                        StringValue = "Canada"
                    },
                    new QueryParameterXml()
                    {
                        Name="@UnusedParam",
                        StringValue = "Useless"
                    }
                }
                );

            cmd.Connection.Open();
            var dr = cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);

            Assert.That(dr.Read(), Is.True);
            Assert.That(dr.GetValue(1), Is.EqualTo("Canada"));
            Assert.That(dr.Read(), Is.False);
        }
    }
}
