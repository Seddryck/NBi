#region Using directives
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
            var mae = new MemberAdomdEngine();
            var amc = new AdomdMemberCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Easy Finances";
            amc.Path="[Counter Party].[Accounts Structure].[Label]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("Caption"), Has.None.EqualTo("All"));
            Assert.That(List.Map(actual).Property("Caption"), Has.Member(""));
            Assert.That(List.Map(actual).Property("Caption"), Has.Member("Aula Magna"));
        }

        [Test]
        public void GetMembersByLevel_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectUniqueName()
        {
            //Buiding object used during test
            var mae = new MemberAdomdEngine();
            var amc = new AdomdMemberCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Easy Finances";
            amc.Path = "[Counter Party].[Accounts Structure].[Label]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("UniqueName"), Has.Member("[Counter Party].[Accounts Structure].[Label].&[]"));
            Assert.That(List.Map(actual).Property("UniqueName"), Has.Member("[Counter Party].[Accounts Structure].[Label].&[Aula Magna]"));
        }

        [Test]
        public void GetMembersByLevel_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectOrdinal()
        {
            //Buiding object used during test
            var mae = new MemberAdomdEngine();
            var amc = new AdomdMemberCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Easy Finances";
            amc.Path = "[Counter Party].[Accounts Structure].[Label]";

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
            var mae = new MemberAdomdEngine();
            var amc = new AdomdMemberCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Easy Finances";
            amc.Path = "[Counter Party].[Accounts Structure].[Label]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("LevelNumber"), Has.All.EqualTo(1));
        }

        [Test]
        public void GetMembersByHierarchy_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectCaptions()
        {
            //Buiding object used during test
            var mae = new MemberAdomdEngine();
            var amc = new AdomdMemberCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Easy Finances";
            amc.Path = "[Counter Party].[Accounts Structure]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("Caption"), Has.Member("All"));
            Assert.That(List.Map(actual).Property("Caption"), Has.Member(""));
            Assert.That(List.Map(actual).Property("Caption"), Has.Member("Aula Magna"));
        }

        [Test]
        public void GetMembersByHierarchy_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectUniqueName()
        {
            //Buiding object used during test
            var mae = new MemberAdomdEngine();
            var amc = new AdomdMemberCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Easy Finances";
            amc.Path = "[Counter Party].[Accounts Structure]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(List.Map(actual).Property("UniqueName"), Has.Member("[Counter Party].[Accounts Structure].[Label].&[]"));
            Assert.That(List.Map(actual).Property("UniqueName"), Has.Member("[Counter Party].[Accounts Structure].[Label].&[Aula Magna]"));
        }

        [Test]
        public void GetMembersByHierarchy_CounterPartyAccountsStructureLabel_ReturnListMembersWithCorrectOrdinal()
        {
            //Buiding object used during test
            var mae = new MemberAdomdEngine();
            var amc = new AdomdMemberCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Easy Finances";
            amc.Path = "[Counter Party].[Accounts Structure]";

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
            var mae = new MemberAdomdEngine();
            var amc = new AdomdMemberCommand(ConnectionStringReader.GetAdomd());
            amc.Perspective = "Easy Finances";
            amc.Path = "[Counter Party].[Accounts Structure]";

            //Call the method to test
            var actual = mae.Execute(amc);

            //Assertion
            Assert.That(actual, Has.Count.GreaterThan(0));
            Assert.That(List.Map(actual).Property("LevelNumber"), Has.All.LessThanOrEqualTo(2));
        }
    }
}
