using NUnit.Framework;
using NBi.Core.Query;

namespace NBi.Testing.Unit.Core.Analysis.Query
{
    [TestFixture]
    public class OleDbExecutorTest
    {
        [Test]
        public void Execute_ValidMdx_GetResult()
        {
            var oe = new QueryOleDbEngine("Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";");
            var ds = oe.Execute("SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]");

            Assert.IsInstanceOf<string>(ds.Tables[0].Rows[0][0]);
            Assert.AreEqual((string)ds.Tables[0].Rows[0][0], "2009");
            Assert.AreEqual((string)ds.Tables[0].Rows[1][0], "2010");
            Assert.IsInstanceOf<double>(ds.Tables[0].Rows[1][1]);
        }
    }
}
