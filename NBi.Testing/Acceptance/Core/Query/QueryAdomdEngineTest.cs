using NBi.Core.Query;
using NUnit.Framework;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Testing.Acceptance.Core.Query
{
    [TestFixture]
    public class QueryAdomdEngineTest
    {
        [Test]
        public void Execute_ValidMdx_GetResult()
        {

            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new QueryAdomdEngine(cmd);
            var ds = qe.Execute();

            Assert.IsInstanceOf<string>(ds.Tables[0].Rows[0][0]);
            Assert.AreEqual((string)ds.Tables[0].Rows[0][0], "2009");
            Assert.AreEqual((string)ds.Tables[0].Rows[1][0], "2010");
            Assert.IsInstanceOf<double>(ds.Tables[0].Rows[1][1]);
        }

    }
}
