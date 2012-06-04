using NBi.Core.Analysis.Member;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.Core.Analysis.Member
{
    [TestFixture]
    public class CubeDimensionAdomdEngineTest
    {
        [Test]
        public void GetMembers_ExistingDimension_ListOfMembers()
        {
            var connString = ConnectionStringReader.GetAdomd();
            var disco = new DiscoverMemberCommand(connString);
            disco.Path = "[Date].[Calendar].[Year]";
            disco.Perspective = "Finances";

            var engine = new CubeDimensionAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(4));
        }
    }
}
