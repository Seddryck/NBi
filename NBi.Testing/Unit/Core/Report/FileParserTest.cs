using System;
using System.IO;
using System.Linq;
using System.Reflection;
using NBi.Core.Report;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.Report
{
    [TestFixture]
    public class FileParserTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {
        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
            CreateReportFile("Currency_List");
            CreateReportFile("Currency_Rates");
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {
        }

        #endregion

        protected void CreateReportFile(string filename)
        {
            string file = @"\Temp\" + filename + ".rdl";
            if (File.Exists(file))
                File.Delete(file);

            // A Stream is needed to read the XML document.
            using (Stream stream = Assembly.GetExecutingAssembly()
                                           .GetManifestResourceStream("NBi.Testing.Unit.Core.Report.Resources." + filename + ".rdl"))
            using (StreamReader reader = new StreamReader(stream))
            {
                File.WriteAllText(file, reader.ReadToEnd());
            }
            
        }

        [Test]
        public void ExtractQuery_ExistingReportAndDataSet_CorrectQueryReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    @"\Temp\"
                    , "Currency_List"
                    , "Currency"
                );

            var parser = new FileParser();
            var query = parser.ExtractQuery(request);

            Assert.That(query,
                Is.StringContaining("SELECT").And
                .StringContaining("[CurrencyAlternateKey]").And
                .StringContaining("[DimCurrency]"));
        }

        [Test]
        public void ExtractQuery_NonExistingDataSetOneExisting_CorrectExceptionReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    @"\Temp\"
                    , "Currency_List"
                    , "Non Existing"
                );

            var parser = new FileParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("'Currency'"));
        }

        [Test]
        public void ExtractQuery_NonExistingDataSetMoreThanOneExisting_CorrectExceptionReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    @"\Temp\"
                    , "Currency_Rates"
                    , "Non Existing"
                );

            var parser = new FileParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("DataSet1").And.StringContaining("DataSet2"));
        }

        [Test]
        public void ExtractQuery_NonExistingReport_CorrectExceptionReturned()
        {
            var request = new NBi.Core.Report.FileRequest(
                    @"\Temp\"
                    , "Not Existing"
                    , "DataSet1"
                );

            var parser = new FileParser();
            var ex = Assert.Throws<ArgumentException>(() => parser.ExtractQuery(request));
            Assert.That(ex.Message, Is.StringContaining("No report found"));
        }
    }
}
