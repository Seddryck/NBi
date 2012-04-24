#region Using directives

using NUnit.Framework;
using NBi.UI.Configuration;

#endregion

namespace NBi.Testing.UI.Configuration
{
    [TestFixture]
    public class ProjectTest
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
            //Buiding object used during test
            var filename = DiskOnFile.CreatePhysicalFile("MyProject.nbi", "NBi.Testing.UI.Configuration.MyProject.nbi");

            //Call the method to test
            Project.Load(filename);

            //Assertion
            Assert.That(Project.Directories.Root, Is.EqualTo(@"C:\Users\Seddryck\Documents\TestCCH\"));
        }

        [Test]
        public void Load_CorrectFile_DirectoryLoaded()
        {
            //Buiding object used during test
            var filename = DiskOnFile.CreatePhysicalFile("MyProject.nbi", "NBi.Testing.UI.Configuration.MyProject.nbi");

            //Call the method to test
            Project.Load(filename);

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
            //Buiding object used during test
            var filename = DiskOnFile.CreatePhysicalFile("MyProject.nbi", "NBi.Testing.UI.Configuration.MyProject.nbi");

            //Call the method to test
            Project.Load(filename);

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
        public void Save_CorrectFile_DirectoriesSaved()
        {
            //Buiding object used during test
            var filename = DiskOnFile.CreatePhysicalFile("MyProject-Saved.nbi", "NBi.Testing.UI.Configuration.MyProject-Saved.nbi");
            var filenameActual = filename.Replace("-Saved","-Created");

            Project.Directories.Root = @"C:\Users\Seddryck\Documents\TestCCH\";

            Project.Directories[DirectoryCollection.DirectoryType.Metadata].Path =@"Metadata";
            Project.Directories[DirectoryCollection.DirectoryType.Metadata].File =@"Metadata.xls";
            Project.Directories[DirectoryCollection.DirectoryType.Query].Path=@"Query";
            Project.Directories[DirectoryCollection.DirectoryType.Expect].Path=@"Expect";
            Project.Directories[DirectoryCollection.DirectoryType.Actual].Path=@"Actual";
            Project.Directories[DirectoryCollection.DirectoryType.TestSuite].Path=@"TestSuite";

            Project.ConnectionStrings[
                ConnectionStringCollection.ConnectionClass.Adomd,
                ConnectionStringCollection.ConnectionType.Expect
                ].Value = "Data Source=localhost;Catalog=\"Finances Analysis\";";

            Project.ConnectionStrings[
                ConnectionStringCollection.ConnectionClass.Oledb,
                ConnectionStringCollection.ConnectionType.Expect
                ].Value = "Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";";

            Project.ConnectionStrings[
                ConnectionStringCollection.ConnectionClass.Oledb,
                ConnectionStringCollection.ConnectionType.Actual
                ].Value = "Provider=MSOLAP.4;Data Source=localhost;Catalog=\"Finances Analysis\";";

            //Call the method to test
            Project.Save(filenameActual);

            //Assertion
            FileAssert.AreEqual(filename, filenameActual); //Should be changed in something more usefull because failing for unknwon reason
        }

    }
}
