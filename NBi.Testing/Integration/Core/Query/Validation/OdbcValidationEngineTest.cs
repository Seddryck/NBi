using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data.Odbc;
using NBi.Core;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Validation;
using Queryable = NBi.Core.Query;

namespace NBi.Testing.Integration.Core.Query.Validation
{
    [TestFixture]
    public class OdbcValidationEngineTest
    {
        [Test]
        public void Parse_CorrectTableName_Success()
        {
            var Odbc = "SELECT * FROM [HumanResources].[Department];";
            var cmd = new Queryable.Query(Odbc, ConnectionStringReader.GetOdbcSql());
            var qp = new ValidationEngineFactory().Instantiate(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.True);
        }

        [Test]
        public void Parse_WrongTableName_Failed()
        {
            var Odbc = "SELECT * FROM WrongTableName;";
            var cmd = new Queryable.Query(Odbc, ConnectionStringReader.GetOdbcSql());
            var qp = new ValidationEngineFactory().Instantiate(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Invalid object name 'WrongTableName'."));
        }

        [Test]
        public void Parse_CorrectFields_Success()
        {
            var Odbc = "select [DepartmentID], Name from [HumanResources].[Department];";
            var cmd = new Queryable.Query(Odbc, ConnectionStringReader.GetOdbcSql());
            var qp = new ValidationEngineFactory().Instantiate(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.True);
        }

        [Test]
        public void Parse_WrongField_Failed()
        {
            var Odbc = "select [DepartmentID], Name, WrongField from [HumanResources].[Department];";
            var cmd = new Queryable.Query(Odbc, ConnectionStringReader.GetOdbcSql());
            var qp = new ValidationEngineFactory().Instantiate(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Invalid column name 'WrongField'."));
        }

        [Test]
        public void Parse_WrongFields_Failed()
        {
            var Odbc = "select [DepartmentID], Name, WrongField1, WrongField2 from [HumanResources].[Department];";
            var cmd = new Queryable.Query(Odbc, ConnectionStringReader.GetOdbcSql());
            var qp = new ValidationEngineFactory().Instantiate(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors, Has.Member("Invalid column name 'WrongField1'."));
            Assert.That(res.Errors, Has.Member("Invalid column name 'WrongField2'."));
        }

        [Test]
        public void Parse_WrongSyntax_Failed()
        {
            var Odbc = "SELECTION [DepartmentID], Name, WrongField1, WrongField2 from [HumanResources].[Department];";
            var cmd = new Queryable.Query(Odbc, ConnectionStringReader.GetOdbcSql());
            var qp = new ValidationEngineFactory().Instantiate(cmd);

            var res = qp.Parse();

            Assert.That(res.IsSuccesful, Is.False);
            Assert.That(res.Errors[0], Is.EqualTo("Incorrect syntax near 'SELECTION'."));
        }


        [Test]
        public void Parse_DontExecuteEffectivelyQuery()
        {
            var OdbcCount = @"SELECT COUNT(*) from [HumanResources].[Department]";
            var Odbc = @"DELETE from [HumanResources].[Department]";

            var countBefore = ExecuteCount(OdbcCount);
            if (countBefore == 0) //If nothing was present we cannot assert
                Assert.Inconclusive();

            var cmd = new Queryable.Query(Odbc, ConnectionStringReader.GetOdbcSql());
            var qp = new ValidationEngineFactory().Instantiate(cmd);

            var res = qp.Parse();

            if (!res.IsSuccesful)//If syntax is incorrect we cannot assert
                Assert.Inconclusive();

            var countAfter = ExecuteCount(OdbcCount);

            Assert.That(countAfter, Is.EqualTo(countBefore));
        }

        private int ExecuteCount(string OdbcCount)
        {
            int count;

            using (OdbcConnection conn = new OdbcConnection(ConnectionStringReader.GetOdbcSql()))
            {
                conn.Open();

                using (OdbcCommand cmd = new OdbcCommand(OdbcCount, conn))
                {
                    count = (int)cmd.ExecuteScalar();
                }

                if (conn.State != System.Data.ConnectionState.Closed)
                    conn.Close();
            }

            return count;
        }
    }
}
