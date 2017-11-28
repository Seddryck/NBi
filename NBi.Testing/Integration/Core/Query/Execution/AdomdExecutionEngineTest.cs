using System;
using NBi.Core.Query;
using NUnit.Framework;
using NBi.Core.Query.Execution;
using Microsoft.AnalysisServices.AdomdClient;

namespace NBi.Testing.Integration.Core.Query.Execution
{
    [TestFixture]
    [Category("Olap")]
    public class AdomdExecutionEngineTest
    {
        [Test]
        public void Execute_ValidQuery_DataSetFilled()
        {
            var query = "SELECT [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));
            var qe = new AdomdExecutionEngine(cmd);
            var ds = qe.Execute();

            Assert.IsInstanceOf<string>(ds.Tables[0].Rows[0][0]);
            Assert.AreEqual((string)ds.Tables[0].Rows[0][0], "CY 2005");
            Assert.AreEqual((string)ds.Tables[0].Rows[1][0], "CY 2006");
            Assert.IsInstanceOf<double>(ds.Tables[0].Rows[1][1]);
        }

        [Test]
        public void Execute_ValidMdxWithNull_GetResult()
        {
            var query = "SELECT  [Measures].[Amount] ON 0, [Date].[Calendar].[Calendar Year].&[2010] ON 1 FROM [Adventure Works]";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomd()));
            var qe = new AdomdExecutionEngine(cmd);
            var ds = qe.Execute();

            Assert.IsInstanceOf<string>(ds.Tables[0].Rows[0][0]);
            Assert.AreEqual((string)ds.Tables[0].Rows[0][0], "CY 2010");
            Assert.IsInstanceOf<DBNull>(ds.Tables[0].Rows[0][1]);
            Assert.That(ds.Tables[0].Rows[0].IsNull(1), Is.True);
        }

        [Test]
        public void Execute_ValidDax_GetResult()
        {
            var query = "EVALUATE CALCULATETABLE(VALUES('Product Subcategory'[Product Subcategory Name]),'Product Category'[Product Category Name] = \"Bikes\")";
            var cmd = new AdomdCommand(query, new AdomdConnection(ConnectionStringReader.GetAdomdTabular()));
            var qe = new AdomdExecutionEngine(cmd);
            var ds = qe.Execute();

            Assert.IsInstanceOf<string>(ds.Tables[0].Rows[0][0]);
            Assert.AreEqual((string)ds.Tables[0].Rows[0][0], "Mountain Bikes");
            Assert.AreEqual((string)ds.Tables[0].Rows[1][0], "Road Bikes");
            Assert.AreEqual((string)ds.Tables[0].Rows[2][0], "Touring Bikes");

            Assert.AreEqual(ds.Tables[0].Rows.Count, 3);
            Assert.AreEqual(ds.Tables[0].Columns.Count, 1);
        }
    }
}
