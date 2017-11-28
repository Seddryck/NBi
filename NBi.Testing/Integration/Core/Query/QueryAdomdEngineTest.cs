using System;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Query;
using NUnit.Framework;
using NBi.Core.Query.Execution;

namespace NBi.Testing.Integration.Core.Query
{
    [TestFixture]
    [Category("Olap")]
    public class QueryAdomdEngineTest
    {
        [Test]
        public void Parse_ValidMdx_Success()
        {

            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new QueryAdomdEngine(cmd);
            var pr = qe.Parse();

            Assert.That(pr.IsSuccesful, Is.True);
        }

        [Test]
        public void Parse_NotValidMdx_Failed()
        {

            var query = "SELECT  [Measures].[NonEXisting] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new QueryAdomdEngine(cmd);
            var pr = qe.Parse();

            Assert.That(pr.IsSuccesful, Is.False);
        }

        [Test]
        public void ExecuteCellSet_ValidMdx_CellSet()
        {
            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new QueryAdomdEngine(cmd);
            var cellSet = qe.ExecuteCellSet();

            Assert.That(cellSet, Is.InstanceOf<CellSet>());

            Assert.That(cellSet.Cells[0,0].Value, Is.InstanceOf<double>());
            Assert.That(cellSet.Cells[0, 0].Value, Is.EqualTo(1874469));

            Assert.That(cellSet.Axes[1].Positions[0].Members[0].Caption, Is.EqualTo("CY 2005"));
            Assert.That(cellSet.Axes[1].Positions[1].Members[0].Caption, Is.EqualTo("CY 2006"));
        }

        [Test]
        public void ExecuteCellSet_ValidMdx_CellSetWithFormatProperties()
        {
            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));

            var qe = new QueryAdomdEngine(cmd);
            var cellSet = qe.ExecuteCellSet();

            var cell = cellSet.Cells[0, 0];
            Assert.That(cell.FormattedValue, Is.EqualTo("$1,874,469.00"));
        }

    }
}
