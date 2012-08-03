using NBi.Core.Analysis;
using NBi.Core.Analysis.Member;
using NUnit.Framework;

namespace NBi.Testing.Acceptance.Core.Analysis.Member
{
    [TestFixture]
    public class SchemaRowsetAdomdEngineTest
    {
        [Test]
        public void GetMembers_ExistingDimension_ListOfMembers()
        {
            var connString = ConnectionStringReader.GetAdomd();
            var disco = new DiscoverCommand(connString);
            disco.Path = "[Date].[Calendar].[Calendar Year]";
            disco.Perspective = "Adventure Works";

            var engine = new SchemaRowsetAdomdEngine();
            var res = engine.Execute(disco);

            Assert.That(res.Count, Is.EqualTo(5));
        }

        [Test]
        public void GetMembers_ExistingLevelChildren_ThrowsArgumentException()
        {
            var connString = ConnectionStringReader.GetAdomd();
            var disco = new DiscoverCommand(connString);
            disco.Path = "[Date].[Calendar].[Calendar Year].[CY 2003]";
            disco.Function = "children";
            disco.Perspective = "Adventure Works";

            var engine = new SchemaRowsetAdomdEngine();

            Assert.Throws<System.ArgumentException>(delegate {engine.Execute(disco);});
        }
    }
}
