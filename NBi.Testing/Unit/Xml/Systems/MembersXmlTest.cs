#region Using directives
using NBi.Core.Analysis.Discovery;
using NBi.Xml.Systems;
using NBi.Xml.Systems.Structure;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml.Systems
{
    [TestFixture]
    public class MembersXmlTest
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

        [Ignore]
        [Test]
        public void Instantiate_ChildrenOfFilled_MemberCaptionIsSet()
        {
            //Buiding object used during test
            var xml = new LevelXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "perspective";
            xml.Dimension = "dimension";
            xml.Hierarchy = "hierarchy";
            xml.Caption = "level";
            xml.Members = new MembersXml();
            xml.Members.ChildrenOf = "parent-member";

            //Call the method to test
            var actual = (MembersDiscoveryCommand)(xml.Instantiate());

            //Assertion
            Assert.That(actual.MemberCaption, Is.EqualTo("parent-member"));
        }

        [Ignore]
        [Test]
        public void Instantiate_ChildrenOfFilled_FunctionIsSetToChildren()
        {
            //Buiding object used during test
            var xml = new LevelXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "perspective";
            xml.Dimension = "dimension";
            xml.Hierarchy = "hierarchy";
            xml.Caption = "level";
            xml.Members = new MembersXml();
            xml.Members.ChildrenOf = "children";

            //Call the method to test
            var actual = (MembersDiscoveryCommand)(xml.Instantiate());

            //Assertion
            Assert.That(actual.Function.ToLower(), Is.EqualTo("Children".ToLower()));
        }

        [Ignore]
        [Test]
        public void Instantiate_ChildrenOfNotFilled_FunctionIsSetToMembers()
        {
            //Buiding object used during test
            var xml = new LevelXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "perspective";
            xml.Dimension = "dimension";
            xml.Hierarchy = "hierarchy";
            xml.Caption = "level";
            xml.Members = new MembersXml();

            //Call the method to test
            var actual = (MembersDiscoveryCommand)(xml.Instantiate());

            //Assertion
            Assert.That(actual.Function.ToLower(), Is.EqualTo("Members".ToLower()));
        }



    }
}
