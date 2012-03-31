using NUnit.Framework;
using NBi.QueryGenerator;
using System.IO;

namespace NBi.Testing.QueryGenerator
{
    [TestFixture]
    public class ResultSetWriterTest
    {
        [Test]
        public void XmlWrite_DataSet_FileCreated()
        {
            if (File.Exists(@"C:\Users\Seddryck\Documents\TestCCH\TestWrite.xml"))
                File.Delete(@"C:\Users\Seddryck\Documents\TestCCH\TestWrite.xml");
            
            var oe = new OleDbExecutor("Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";");
            var ds = oe.Execute("SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]");

            var xrsw = new XmlResultSetWriter(@"C:\Users\Seddryck\Documents\TestCCH\");
            xrsw.Write("TestWrite.xml", ds);

            Assert.True(File.Exists(@"C:\Users\Seddryck\Documents\TestCCH\TestWrite.xml"));
        }

        [Test]
        public void CsvWrite_DataSet_FileCreated()
        {
            if (File.Exists(@"C:\Users\Seddryck\Documents\TestCCH\TestWrite.csv"))
                File.Delete(@"C:\Users\Seddryck\Documents\TestCCH\TestWrite.csv");

            var oe = new OleDbExecutor("Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";");
            var ds = oe.Execute("SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]");

            var crsw = new CsvResultSetWriter(@"C:\Users\Seddryck\Documents\TestCCH\");
            crsw.Write("TestWrite.csv", ds);

            Assert.True(File.Exists(@"C:\Users\Seddryck\Documents\TestCCH\TestWrite.csv"));
        }
    }
}
