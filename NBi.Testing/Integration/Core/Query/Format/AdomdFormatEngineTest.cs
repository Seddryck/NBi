using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query;
using NUnit.Framework;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Format;
using System.Linq;

namespace NBi.Testing.Integration.Core.Query.Format
{
    [TestFixture]
    [Category("Olap")]
    public class AdomdFormatEngineTest
    {
        [Test]
        public void ExecuteFormat_FormattedDouble_String()
        {
            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new AdomdFormatEngine(cmd);
            var result = qe.ExecuteFormat();

            Assert.That(result.ElementAt(0), Is.EqualTo("$1,874,469.00"));
        }
    }
}
