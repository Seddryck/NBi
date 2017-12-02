using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query;
using NUnit.Framework;
using System.Data.Odbc;
using NBi.Core;
using NBi.Core.Query.Execution;
using NBi.Core.Query.Validation;

namespace NBi.Testing.Integration.Core.Query.Validation
{
    [TestFixture]
    [Category("Olap")]
    public class QueryAdomdEngineTest
    {
        [Test]
        public void Parse_ValidMdx_Successful()
        {
            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));
            var qp = new ValidationEngineFactory().Instantiate(cmd);
            var result = qp.Parse();
            Assert.That(result.IsSuccesful, Is.True);
        }

        [Test]
        public void Parse_NotValidMdx_NotSuccessful()
        {
            var query = "SELECT  [Measures].[NonEXisting] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));
            var qp = new ValidationEngineFactory().Instantiate(cmd);
            var result = qp.Parse();
            Assert.That(result.IsSuccesful, Is.False);
        }
    }
}
