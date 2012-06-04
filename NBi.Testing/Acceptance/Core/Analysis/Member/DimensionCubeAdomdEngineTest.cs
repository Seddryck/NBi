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
            var disco = new DiscoverMemberCommand(connString);
            disco.Path = "[Date].[Calendar].[Year]";

            var engine = new DimensionCubeAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(4));
        }
    }
}
