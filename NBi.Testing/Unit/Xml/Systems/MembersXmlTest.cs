#region Using directives

using NUnit.Framework;
using NBi.Xml.Systems;
using NBi.Core.Analysis;

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
        public void Instantiate_ChildrenOfFilled_PathIncludesChildrenOf()
        {
            //Buiding object used during test
            var xml = new MembersXml();
            xml.Path = "[dimension].[hierarchy].[level]";
            xml.ChildrenOf = "parent";

            //Call the method to test
            var actual = (DiscoverCommand)(xml.Instantiate());

            //Assertion
            Assert.That(actual.Path, Is.EqualTo("[dimension].[hierarchy].[level].[parent]"));
        }

        [Test]
        public void Instantiate_ChildrenOfFilled_FunctionIsSetToChildren()
        {
            //Buiding object used during test
            var xml = new MembersXml();
            xml.Path = "[dimension].[hierarchy].[level]";
            xml.ChildrenOf = "parent";

            //Call the method to test
            var actual = (DiscoverCommand)(xml.Instantiate());

            //Assertion
            Assert.That(actual.Function, Is.EqualTo("children"));
        }

        [Test]
        public void Instantiate_ChildrenOfNotFilled_FunctionIsSetToMembers()
        {
            //Buiding object used during test
            var xml = new MembersXml();
            xml.Path = "[dimension].[hierarchy].[level]";

            //Call the method to test
            var actual = (DiscoverCommand)(xml.Instantiate());

            //Assertion
            Assert.That(actual.Function, Is.EqualTo("members"));
        }



    }
}
