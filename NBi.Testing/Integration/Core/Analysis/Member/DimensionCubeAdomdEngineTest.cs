using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Member
{
    [TestFixture]
    public class DimensionCubeAdomdEngineTest
    {
        [Test]
        public void GetMembers_ExistingDimension_ListOfMembers()
        {
            var connString = ConnectionStringReader.GetAdomd();
            var disco = new DiscoverCommand(connString);
            disco.Path = "[Date].[Calendar].[Calendar Year]";
            disco.Function = "members";

            var engine = new DimensionCubeAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(5));
        }

        [Test]
        public void GetMembers_ExistingLevelChildren_ListOfMembers()
        {
            var connString = ConnectionStringReader.GetAdomd();
            var disco = new DiscoverCommand(connString);
            disco.Path = "[Date].[Calendar].[Calendar Year].[CY 2003]";
            disco.Function = "children";
            disco.Perspective = "Adventure Works";

            var engine = new DimensionCubeAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(2));
        }
    }
}
