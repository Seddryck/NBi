using NBi.Core.Analysis.Discovery;
using NBi.Core.Analysis.Member;
using NUnit.Framework;

namespace NBi.Testing.Integration.Core.Analysis.Member
{
    [TestFixture]
    public class SchemaRowsetAdomdEngineTest
    {
        [Test]
        public void GetMembers_ExistingDimension_ListOfMembers()
        {
            var disco = DiscoveryFactory.BuildForMembers(
                ConnectionStringReader.GetAdomd(),
                "Adventure Works",
                "[Date].[Calendar].[Calendar Year]");

            var engine = new SchemaRowsetAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(5));
        }

        [Test]
        public void GetMembers_ExistingLevelChildren_ThrowsArgumentException()
        {
            var disco = DiscoveryFactory.BuildForMembers(
                ConnectionStringReader.GetAdomd(),
                "Adventure Works",
                "[Date].[Calendar].[Calendar Year]",
                "CY 2003");

            var engine = new SchemaRowsetAdomdEngine();

            Assert.Throws<System.ArgumentException>(delegate {engine.Execute(disco);});
        }
    }
}
