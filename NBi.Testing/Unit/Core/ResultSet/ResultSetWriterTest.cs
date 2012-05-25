using System.Data;
using System.IO;
using System.Reflection;
using NBi.Core.ResultSet;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class ResultSetWriterTest
    {
        [Test]
        public void XmlWrite_DataSet_FileCreated()
        {
            //Clean up before testing
            var filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestWrite.xml";
            if (File.Exists(filename))
                File.Delete(filename);

            //Acquire data to perform test
            var ds = new DataSet();
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Core.ResultSet.Resources.ResultDataSet.xml"))
            {
                //Load the file content into the dataset
                ds.ReadXml(stream);
            }

            //Create the object to test
            var xrsw = new ResultSetXmlWriter(Path.GetDirectoryName(filename));
            xrsw.Write(Path.GetFileName(filename), ds);

            //Assertion
            Assert.True(File.Exists(filename));
        }

        [Test]
        public void CsvWrite_DataSet_FileCreated()
        {
            //Clean up before testing
            var filename = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + @"\TestWrite.csv";
            if (File.Exists(filename))
                File.Delete(filename);

            //Acquire data to perform test
            var ds = new DataSet();
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream( "NBi.Testing.Unit.Core.ResultSet.Resources.ResultDataSet.xml"))
            {
                //Load the file content into the dataset
                ds.ReadXml(stream);
            }
            

            //Create the object to test
            var crsw = new ResultSetCsvWriter(Path.GetDirectoryName(filename));
            crsw.Write(Path.GetFileName(filename), ds);

            //Assertion
            Assert.True(File.Exists(filename));
        }
    }
}
