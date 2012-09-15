#region Using directives
using NBi.Core.Analysis.Discovery;
using NBi.Xml.Systems;
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

        [Test]
        public void Instantiate_ChildrenOfFilled_MemberCaptionIsSet()
        {
            //Buiding object used during test
            var xml = new MembersXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "perspective";
            xml.Path = "[dimension].[hierarchy].[level]";
            xml.ChildrenOf = "parent";

            //Call the method to test
            var actual = (MembersDiscoveryCommand)(xml.Instantiate());

            //Assertion
            Assert.That(actual.MemberCaption, Is.EqualTo("parent"));
        }

        [Test]
        public void Instantiate_ChildrenOfFilled_FunctionIsSetToChildren()
        {
            //Buiding object used during test
            var xml = new MembersXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "perspective";
            xml.Path = "[dimension].[hierarchy].[level]";
            xml.ChildrenOf = "children";

            //Call the method to test
            var actual = (MembersDiscoveryCommand)(xml.Instantiate());

            //Assertion
            Assert.That(actual.Function.ToLower(), Is.EqualTo("Children".ToLower()));
        }

        [Test]
        public void Instantiate_ChildrenOfNotFilled_FunctionIsSetToMembers()
        {
            //Buiding object used during test
            var xml = new MembersXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "perspective";
            xml.Path = "[dimension].[hierarchy].[level]";

            //Call the method to test
            var actual = (MembersDiscoveryCommand)(xml.Instantiate());

            //Assertion
            Assert.That(actual.Function.ToLower(), Is.EqualTo("Members".ToLower()));
        }



    }
}
