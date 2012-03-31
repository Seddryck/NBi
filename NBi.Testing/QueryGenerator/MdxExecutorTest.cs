using NBi.QueryGenerator;
using NUnit.Framework;

namespace NBi.Testing.QueryGenerator
{
    [TestFixture]
    public class MdxExecutorTest
    {
        [Test]
        public void Execute_ValidMdx_GetResult()
        {
            var oe = new OleDbExecutor("Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";");
            var ds = oe.Execute("SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]");

            Assert.AreEqual((string)ds.Tables[0].Rows[0][0], "2009");
        }
    }
}
