using System.IO;
using System.Reflection;
using NBi.NUnit;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Xml
{
    [TestFixture]
    public class QueryXmlTest
    {

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
           
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void GetQuery_FilenameSpecified_RetrieveContentOfFile()
        {
            //create a text file on disk
            var filename = DiskOnFile.CreatePhysicalFile("QueryFile.sql", "NBi.Testing.Unit.Xml.Resources.QueryFile.sql");
           
            //Instantiate a Test Case and specify to find the sql in the fie created above
            var testCase = new QueryXml() { File = filename };

            // A Stream is needed to read the text file from the assembly.
            string expectedContent;
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Xml.Resources.QueryFile.sql"))
                using (StreamReader reader = new StreamReader(stream))
                   expectedContent = reader.ReadToEnd();
            
            Assert.AreEqual(expectedContent, testCase.GetQuery());
        }

        [Test]
        public void GetQuery_FilenameNotSpecified_RetrieveContentOfInlineQuery()
        {
            //Instantiate a System Under Test
            var systemUnderTest = new QueryXml() { InlineQuery = "SELECT * FROM Product" };

            Assert.That(systemUnderTest.GetQuery(), Is.EqualTo("SELECT * FROM Product"));
            Assert.That(systemUnderTest.InlineQuery, Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            Assert.That(systemUnderTest.File, Is.Null);
        }

        [Test]
        public void GetQuery_FileNameSpecified_RetrieveContentOfFile()
        {
            //Create the queryfile to read
            var filename = "Select all products.sql";
            DiskOnFile.CreatePhysicalFile(filename, "NBi.Testing.Unit.Xml.Resources.SelectAllProducts.sql");

            var systemUnderTest = new QueryXml() { File = filename };

            // Check the properties of the object.
            Assert.That(systemUnderTest.File, Is.Not.Null.And.Not.Empty);
            Assert.That(systemUnderTest.InlineQuery, Is.Null);
            Assert.That(systemUnderTest.GetQuery(), Is.Not.Null.And.Not.Empty.And.ContainsSubstring("SELECT"));
            
        }


        [Test]
        public void GetQuery_FilenameSpecified_RetrieveContentWithEuroSymbol()
        {
            //create a text file on disk
            var filename = DiskOnFile.CreatePhysicalFile("QueryFile€.mdx", "NBi.Testing.Unit.Xml.Resources.QueryFile€.mdx");

            //Instantiate a Test Case and specify to find the sql in the fie created above
            var testCase = new QueryXml() { File = filename };

            // A Stream is needed to read the text file from the assembly.
            string expectedContent = "select [measure].[price €/Kg] on 0;";

            Assert.AreEqual(expectedContent, testCase.GetQuery());
        }
       
    }
}