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

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new QueryAdomdEngine(cmd);
            var ds = qe.Execute();

            Assert.IsInstanceOf<string>(ds.Tables[0].Rows[0][0]);
            Assert.AreEqual((string)ds.Tables[0].Rows[0][0], "CY 2001");
            Assert.AreEqual((string)ds.Tables[0].Rows[1][0], "CY 2002");
            Assert.IsInstanceOf<double>(ds.Tables[0].Rows[1][1]);
        }

        [Test]
        public void Execute_ValidMdxWithNull_GetResult()
        {

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2006] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new QueryAdomdEngine(cmd);
            var ds = qe.Execute();

            Assert.IsInstanceOf<string>(ds.Tables[0].Rows[0][0]);
            Assert.AreEqual((string)ds.Tables[0].Rows[0][0], "CY 2006");
            
            Assert.IsInstanceOf<System.DBNull>(ds.Tables[0].Rows[0][1]);
            Assert.That(ds.Tables[0].Rows[0].IsNull(1), Is.True);
        }

        [Test]
        public void Parse_ValidMdx_Success()
        {

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2006] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new QueryAdomdEngine(cmd);
            var pr = qe.Parse();

            Assert.That(pr.IsSuccesful, Is.True);
        }

        [Test]
        public void Parse_NotValidMdx_Failed()
        {

            var query = "SELECT  [Measures].[NonEXisting] ON 0, [Date].[Calendar].[Calendar Year].&[2006] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new QueryAdomdEngine(cmd);
            var pr = qe.Parse();

            Assert.That(pr.IsSuccesful, Is.False);
        }

    }
}
