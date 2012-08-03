#region Using directives
using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
using NUnit.Framework;

#endregion

namespace NBi.Testing.Acceptance.Core.Analysis
{
    [TestFixture]
    public class MemberAdomdExtractorTest
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
        public void GetMembersByLevel_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectCaptions()
        {
            //Buiding object used during test
            var mae = new SchemaRowsetAdomdEngine();
            var amc = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Adventure Works";
            amc.Path = "[Geography].[Geography].[Country]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("Caption"), Has.None.EqualTo("All"));
            Assert.That(List.Map(actual).Property("Caption"), Has.Member("Canada"));
            Assert.That(List.Map(actual).Property("Caption"), Has.Member("France"));
        }

        [Test]
        public void GetMembersByLevel_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectUniqueName()
        {
            //Buiding object used during test
            var mae = new SchemaRowsetAdomdEngine();
            var amc = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Adventure Works";
            amc.Path = "[Geography].[Geography].[Country]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("UniqueName"), Has.Member("[Geography].[Geography].[Country].&[Canada]"));
            Assert.That(List.Map(actual).Property("UniqueName"), Has.Member("[Geography].[Geography].[Country].&[France]"));
        }

        [Test]
        public void GetMembersByLevel_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectOrdinal()
        {
            //Buiding object used during test
            var mae = new SchemaRowsetAdomdEngine();
            var amc = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Adventure Works";
            amc.Path = "[Geography].[Geography].[Country]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("Ordinal"), Is.Unique);
            Assert.That(List.Map(actual).Property("Ordinal"), Has.All.GreaterThan(0));
        }

        [Test]
        public void GetMembersByLevel_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectLevelNumber()
        {
            //Buiding object used during test
            var mae = new SchemaRowsetAdomdEngine();
            var amc = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Adventure Works";
            amc.Path = "[Geography].[Geography].[Country]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("LevelNumber"), Has.All.EqualTo(1));
        }

        [Test]
        public void GetMembersByHierarchy_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectCaptions()
        {
            //Buiding object used during test
            var mae = new SchemaRowsetAdomdEngine();
            var amc = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Adventure Works";
            amc.Path = "[Geography].[Geography]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("Caption"), Has.Member("All Geographies"));
            Assert.That(List.Map(actual).Property("Caption"), Has.Member("Canada"));
            Assert.That(List.Map(actual).Property("Caption"), Has.Member("France"));
        }

        [Test]
        public void GetMembersByHierarchy_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectUniqueName()
        {
            //Buiding object used during test
            var mae = new SchemaRowsetAdomdEngine();
            var amc = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Adventure Works";
            amc.Path = "[Geography].[Geography]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("UniqueName"), Has.Member("[Geography].[Geography].[Country].&[Canada]"));
            Assert.That(List.Map(actual).Property("UniqueName"), Has.Member("[Geography].[Geography].[City].&[Toronto]&[ON]"));
        }

        [Test]
        public void GetMembersByHierarchy_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectOrdinal()
        {
            //Buiding object used during test
            var mae = new SchemaRowsetAdomdEngine();
            var amc = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Adventure Works";
            amc.Path = "[Geography].[Geography]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(actual, Has.Count.GreaterThan(0));
            Assert.That(List.Map(actual).Property("Ordinal"), Is.Unique);
            Assert.That(List.Map(actual).Property("Ordinal"), Has.All.GreaterThanOrEqualTo(0));
        }

        [Test]
        public void GetMembersByHierarchy_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectLevelNumber()
        {
            //Buiding object used during test
            var mae = new SchemaRowsetAdomdEngine();
            var amc = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Adventure Works";
            amc.Path = "[Geography].[Geography]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(actual, Has.Count.GreaterThan(0));
            //0 = All
            Assert.That(List.Map(actual).Property("LevelNumber"), Has.Some.EqualTo(0));
            //1 = Country
            Assert.That(List.Map(actual).Property("LevelNumber"), Has.Some.EqualTo(1));
            //2 = State/Province
            Assert.That(List.Map(actual).Property("LevelNumber"), Has.Some.EqualTo(2));
            //3 = Town
            Assert.That(List.Map(actual).Property("LevelNumber"), Has.Some.EqualTo(3));
            //4 = Zip code
            Assert.That(List.Map(actual).Property("LevelNumber"), Has.Some.EqualTo(4));
            //Nothing else 
            Assert.That(List.Map(actual).Property("LevelNumber"), Has.All.LessThanOrEqualTo(4));
            
            
            
            
            
        }
    }
}
