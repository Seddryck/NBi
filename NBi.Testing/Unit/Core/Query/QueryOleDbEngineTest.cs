using System.Data.OleDb;
using NBi.Core.Query;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Query
{
    [TestFixture]
    public class QueryOleDbEngineTest
    {
        [Test]
        public void Execute_ValidMdx_GetResult()
        {


            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
            var cmd = new OleDbCommand(query, new OleDbConnection(ConnectionStringReader.GetOleDb()));

            var qe = new QueryEngineFactory().GetExecutor(cmd);
            var ds = qe.Execute();

            Assert.IsInstanceOf<string>(ds.Tables[0].Rows[0][0]);
            Assert.AreEqual((string)ds.Tables[0].Rows[0][0], "2009");
            Assert.AreEqual((string)ds.Tables[0].Rows[1][0], "2010");
            Assert.IsInstanceOf<double>(ds.Tables[0].Rows[1][1]);
        }
    }
}
