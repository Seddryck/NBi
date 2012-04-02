using NUnit.Framework;
using NBi.Core.Analysis.Query;
using System.IO;
using System.Reflection;

namespace NBi.Testing.Core.Analysis.Query
{
    [TestFixture]
    public class ResultSetWriterTest
    {
        [Test]
        public void XmlWrite_DataSet_FileCreated()
        {
            var filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestWrite.xml";
            
            if (File.Exists(filename))
                File.Delete(filename);
            
            var oe = new OleDbExecutor("Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";");
            var ds = oe.Execute("SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]");

            var xrsw = new XmlResultSetWriter(Path.GetDirectoryName(filename));
            xrsw.Write(Path.GetFileName(filename), ds);

            Assert.True(File.Exists(filename));
        }

        [Test]
        public void CsvWrite_DataSet_FileCreated()
        {
            var filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestWrite.csv";

            if (File.Exists(filename))
                File.Delete(filename);

            var oe = new OleDbExecutor("Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";");
            var ds = oe.Execute("SELECT [Measures].[Amount] ON 0, [Date].[Calendar].Children ON 1 FROM [Finances]");

            var crsw = new CsvResultSetWriter(Path.GetDirectoryName(filename));
            crsw.Write(Path.GetFileName(filename), ds);

            Assert.True(File.Exists(filename));
        }
    }
}
