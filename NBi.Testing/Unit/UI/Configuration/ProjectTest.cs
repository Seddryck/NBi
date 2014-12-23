#region Using directives
using System.IO;
using NBi.UI.Configuration;
using NUnit.Framework;
using System.Reflection;
#endregion

namespace NBi.Testing.Unit.UI.Configuration
{
    [TestFixture]
    public class ProjectTest
    {
        protected string ProjectFilename { get; set; }
        #region SetUp & TearDown
        //Called only at instance creation
        [TestFixtureSetUp]
        public void SetupMethods()
        {
            ProjectFilename = DiskOnFile.CreatePhysicalFile("MyProject.nbi", "NBi.Testing.Unit.UI.Configuration.Resources.MyProject.nbi");
        }

        //Called only at instance destruction
        [TestFixtureTearDown]
        public void TearDownMethods()
        {
            if (File.Exists(DiskOnFile.GetDirectoryPath() + @"\MyProject.nbi"))
                File.Delete(DiskOnFile.GetDirectoryPath() + @"\MyProject.nbi");
        }

        //Called before each test
        [SetUp]
        public void SetupTest()
        {
        }

        //Called after each test
        [TearDown]
        public void TearDownTest()
        {

        }
        #endregion

        [Test]
        public void Load_CorrectFile_DirectoriesLoaded()
        {
            //Call the method to test
            Project.Load(ProjectFilename);

            //Assertion
            Assert.That(Project.Directories.Root, Is.EqualTo(@"C:\Users\Seddryck\Documents\TestCCH\"));
        }

        [Test]
        public void Load_CorrectFile_DirectoryLoaded()
        {
            //Call the method to test
            Project.Load(ProjectFilename);

            //Assertion
            Assert.That(Project.Directories[DirectoryCollection.DirectoryType.Metadata].FullFileName,
                Is.EqualTo(@"C:\Users\Seddryck\Documents\TestCCH\Metadata\metadata.xls"));
            Assert.That(Project.Directories[DirectoryCollection.DirectoryType.Query].FullFileName,
                Is.EqualTo(@"C:\Users\Seddryck\Documents\TestCCH\Query\"));
            Assert.That(Project.Directories[DirectoryCollection.DirectoryType.Expect].FullFileName,
                Is.EqualTo(@"C:\Users\Seddryck\Documents\TestCCH\Expect\"));
            Assert.That(Project.Directories[DirectoryCollection.DirectoryType.Actual].FullFileName,
                Is.EqualTo(@"C:\Users\Seddryck\Documents\TestCCH\Actual\"));
            Assert.That(Project.Directories[DirectoryCollection.DirectoryType.TestSuite].FullFileName,
                Is.EqualTo(@"C:\Users\Seddryck\Documents\TestCCH\TestSuite\"));
        }

        [Test]
        public void Load_CorrectFile_ConnectionStringLoaded()
        {
            //Call the method to test
            Project.Load(ProjectFilename);

            //Assertion
            Assert.That(Project.ConnectionStrings[
                ConnectionStringCollection.ConnectionClass.Adomd,
                ConnectionStringCollection.ConnectionType.Expect
                ].Value,
                Is.EqualTo("Data Source=localhost;Catalog=\"Finances Analysis\";"));

            Assert.That(Project.ConnectionStrings[
                ConnectionStringCollection.ConnectionClass.Oledb,
                ConnectionStringCollection.ConnectionType.Expect
                ].Value,
                Is.EqualTo("Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";"));

            Assert.That(Project.ConnectionStrings[
                ConnectionStringCollection.ConnectionClass.Oledb,
                ConnectionStringCollection.ConnectionType.Actual
                ].Value,
                Is.EqualTo("Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";"));
        }

        [Test]
        public void Save_Root_CorrectFileContent()
        {
            //Buiding object used during test
            var filename = DiskOnFile.GetDirectoryPath() + @"\MyProject-" + MethodBase.GetCurrentMethod().Name + ".nbi";

            Project.Directories.Root = @"C:\Root\Root\";
            Project.Directories[DirectoryCollection.DirectoryType.Metadata].Path = @"C:\MetadataPath\";

            //Call the method to test
            Project.Save(filename);

            //Assertion
            string content = null;
            using (var sr = File.OpenText(filename))
            {
                content = sr.ReadToEnd();
            }
            Assert.That(content, Is.StringContaining("<directories root=\"C:\\Root\\Root\\\">"));
            Assert.That(content, Is.StringContaining("</directories>"));
        }

        [Test]
        public void Save_DirectoryPathAndFile_CorrectFileContent()
        {
            //Buiding object used during test
            var filename = DiskOnFile.GetDirectoryPath() + @"\MyProject-" + MethodBase.GetCurrentMethod().Name + ".nbi";

            Project.Directories[DirectoryCollection.DirectoryType.Metadata].Path = @"C:\MetadataPath\";
            Project.Directories[DirectoryCollection.DirectoryType.Metadata].File = @"MetadataFile.xls";


            //Call the method to test
            Project.Save(filename);

            //Assertion
            string content = null;
            using (var sr = File.OpenText(filename))
            {
                content = sr.ReadToEnd();
            }
            Assert.That(content, Is.StringContaining("<directory key=\"Metadata\" path=\"C:\\MetadataPath\\\" file=\"MetadataFile.xls\" />"));
        }

        [Test]
        public void Save_DirectoryPath_CorrectFileContent()
        {
            //Buiding object used during test
            var filename = DiskOnFile.GetDirectoryPath() + @"\MyProject-" + MethodBase.GetCurrentMethod().Name + ".nbi";

            Project.Directories[DirectoryCollection.DirectoryType.Metadata].Path = @"C:\MetadataPath\";
            Project.Directories[DirectoryCollection.DirectoryType.Metadata].File = @"";

            //Call the method to test
            Project.Save(filename);

            //Assertion
            string content = null;
            using (var sr = File.OpenText(filename))
            {
                content = sr.ReadToEnd();
            }
            Assert.That(content, Is.StringContaining("<directory key=\"Metadata\" path=\"C:\\MetadataPath\\\" />"));
        }

        [Test]
        public void Save_ConnectionStringOledbExpect_CorrectFileContent()
        {
            //Buiding object used during test
            var filename = DiskOnFile.GetDirectoryPath() + @"\MyProject-" + MethodBase.GetCurrentMethod().Name + ".nbi";

            Project.ConnectionStrings[
                ConnectionStringCollection.ConnectionClass.Oledb,
                ConnectionStringCollection.ConnectionType.Expect
                ].Value = "Oledb+Expect";


            //Call the method to test
            Project.Save(filename);

            //Assertion
            string content = null;
            using (var sr = File.OpenText(filename))
            {
                content = sr.ReadToEnd();
            }
            Assert.That(content, Is.StringContaining("<oledb key=\"Expect\">Oledb+Expect</oledb>"));
        }

        [Test]
        public void Save_ConnectionStringAdomdExpect_CorrectFileContent()
        {
            //Buiding object used during test
            var filename = DiskOnFile.GetDirectoryPath() + @"\MyProject-" + MethodBase.GetCurrentMethod().Name + ".nbi";

            Project.ConnectionStrings[
                ConnectionStringCollection.ConnectionClass.Adomd,
                ConnectionStringCollection.ConnectionType.Expect
                ].Value = "Adomd+Expect";

            //Call the method to test
            Project.Save(filename);

            //Assertion
            string content = null;
            using (var sr = File.OpenText(filename))
            {
                content = sr.ReadToEnd();
            }
            Assert.That(content, Is.StringContaining("<adomd key=\"Expect\">Adomd+Expect</adomd>"));
        }

        [Test]
        public void Save_ConnectionStringOledbActual_CorrectFileContent()
        {
            //Buiding object used during test
            var filename = DiskOnFile.GetDirectoryPath() + @"\MyProject-" + MethodBase.GetCurrentMethod().Name + ".nbi";

            Project.ConnectionStrings[
                ConnectionStringCollection.ConnectionClass.Oledb,
                ConnectionStringCollection.ConnectionType.Actual
                ].Value = "Oledb+Actual";

            //Call the method to test
            Project.Save(filename);

            //Assertion
            string content = null;
            using (var sr = File.OpenText(filename))
            {
                content = sr.ReadToEnd();
            }
            Assert.That(content, Is.StringContaining("<oledb key=\"Actual\">Oledb+Actual</oledb>"));
        }
    }
}
