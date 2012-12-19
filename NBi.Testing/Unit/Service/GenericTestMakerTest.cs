using System;
using System.Collections.Generic;
using System.Linq;
using NBi.Service;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;

namespace NBi.Testing.Unit.Service
{
    [TestFixture]
    public class GenericTestMakerTest
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
        public void Build_ExistsTemplateForOneDimension_CorrectTest()
        {
            var template =
                "<test name=\"In dimension '{2}', hierarchy '{1}', a level '{0}' is existing\">" +
                "    <system-under-test>" +
                "        <structure>" +
                "            <level caption=\"{0}\" dimension=\"{2}\" hierarchy=\"{1}\" perspective=\"{3}\"/>" +
                "        </structure>" +
                "    </system-under-test>" +
                "    <assert>" +
                "        <exists/>" +
                "    </assert>" +
                "</test>";

            var info = new List<string[]>()
                {
                    new string[] 
                    {
                        "myLevel",
                        "myHierarchy",
                        "myDimension",
                        "myPerspective"
                    }
                };

            var gtm = new GenericTestMaker(template);

            var results = gtm.Build(info);

            Assert.That(results, Has.Count.EqualTo(1));

            var result = results.ElementAt(0);
            Assert.That(result.Name, Is.EqualTo("In dimension 'myDimension', hierarchy 'myHierarchy', a level 'myLevel' is existing"));
            Assert.That(result.Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)result.Systems[0]).Item, Is.TypeOf<LevelXml>());
            Assert.That(result.Constraints[0], Is.TypeOf<ExistsXml>());

        }

        [Test]
        public void Build_ExistsTemplateForTwoDimensions_TwoTestsReturned()
        {
            var template =
                "<test name=\"In dimension '{2}', hierarchy '{1}', a level '{0}' is existing\">" +
                "    <system-under-test>" +
                "        <structure>" +
                "            <level caption=\"{0}\" dimension=\"{2}\" hierarchy=\"{1}\" perspective=\"{3}\"/>" +
                "        </structure>" +
                "    </system-under-test>" +
                "    <assert>" +
                "        <exists/>" +
                "    </assert>" +
                "</test>";

            var info = new List<string[]>()
                {
                    new string[] 
                    {
                        "myLevel",
                        "myHierarchy",
                        "myDimension",
                        "myPerspective"
                    },
                    new string[] 
                    {
                        "myOtherLevel",
                        "myHierarchy",
                        "myDimension",
                        "myPerspective"
                    }
                };

            var gtm = new GenericTestMaker(template);

            var results = gtm.Build(info);

            Assert.That(results, Has.Count.EqualTo(2));
        }
    }
}
