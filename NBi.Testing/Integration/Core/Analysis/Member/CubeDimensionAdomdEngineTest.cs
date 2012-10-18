using NBi.Core.Analysis.Discovery;
using NBi.Core.Analysis.Member;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Member
{
    [TestFixture]
    public class CubeDimensionAdomdEngineTest
    {
        [Test]
        public void GetMembers_ExistingDimension_ListOfMembers()
        {
            var disco = DiscoveryFactory.BuildForMembers(
                ConnectionStringReader.GetAdomd(),
                "Adventure Works",
                "[Date].[Calendar].[Calendar Year]"
                );
            var engine = new CubeDimensionAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(5)); //years
        }

        [Test]
        public void GetMembers_ExistingLevelChildren_ListOfMembers()
        {
            var disco = DiscoveryFactory.BuildForMembers(
                ConnectionStringReader.GetAdomd(),
                "Adventure Works",
                "[Date].[Calendar].[Calendar Year]",
                "CY 2003");

            var engine = new CubeDimensionAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(2)); //semestres
        }
    }
}
