using System;
using NBi.Core.Query;
using NUnit.Framework;
using NBi.Core.Query.Execution;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query.Performance;
using System.Diagnostics;

namespace NBi.Testing.Integration.Core.Query.Performance
{
    [TestFixture]
    [Category("Olap")]
    public class AdomdPerformanceEngineTest
    {
        [Test]
        public void Execute_Query_NoTimeout()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qp = new AdomdPerformanceEngine(cmd);
            var res = qp.Execute();
            stopWatch.Stop();
            Assert.That(res.TimeElapsed, Is.LessThanOrEqualTo(stopWatch.Elapsed));
            Assert.That(res.IsTimeOut, Is.False);
        }

        [Test]
        public void CleanCache_Any_ThrowExceptionBecauseNotAllowed()
        {
            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qp = new AdomdPerformanceEngine(cmd);
            qp.CleanCache();
            Assert.Pass();
        }
    }
}
