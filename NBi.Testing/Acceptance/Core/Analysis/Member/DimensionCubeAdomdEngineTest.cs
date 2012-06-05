using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.Core.Analysis.Member
{
    [TestFixture]
    public class DimensionCubeAdomdEngineTest
    {
        [Test]
        public void GetMembers_ExistingDimension_ListOfMembers()
        {
            var connString = ConnectionStringReader.GetAdomd();
            var disco = new DiscoverCommand(connString);
            disco.Path = "[Date].[Calendar].[Year]";
            disco.Function = "members";

            var engine = new DimensionCubeAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(4));
        }

        [Test]
        public void GetMembers_ExistingLevelChildren_ListOfMembers()
        {
            var connString = ConnectionStringReader.GetAdomd();
            var disco = new DiscoverCommand(connString);
            disco.Path = "[Date].[Calendar].[Year].[2010]";
            disco.Function = "children";
            disco.Perspective = "Finances";

            var engine = new DimensionCubeAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(12));
        }
    }
}
