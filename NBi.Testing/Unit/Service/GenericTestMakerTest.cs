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

        private const string TEMPLATE =
                "<test name=\"A level named '{level.caption}' exists in hierarchy '{hierarchy.caption}', dimension '{dimension.caption}'.\">" +
                "    <system-under-test>" +
                "        <structure>" +
                "            <level caption=\"{level.caption}\" dimension=\"{dimension.caption}\" hierarchy=\"{hierarchy.caption}\" perspective=\"{perspective.caption}\"/>" +
                "        </structure>" +
                "    </system-under-test>" +
                "    <assert>" +
                "        <exists/>" +
                "    </assert>" +
                "</test>";

        [Test]
        public void Build_ExistsTemplateForOneDimension_CorrectTest()
        {
            var template = TEMPLATE;

            var variables = new string[]
            {
                "level.caption",
                "hierarchy.caption",
                "dimension.caption",
                "perspective.caption"
            };

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

            var gtm = new GenericTestMaker(template, variables);

            var results = gtm.Build(info);

            Assert.That(results, Has.Count.EqualTo(1));

            var result = results.ElementAt(0);
            Assert.That(result.Name, Is.EqualTo("A level named 'myLevel' exists in hierarchy 'myHierarchy', dimension 'myDimension'."));
            Assert.That(result.Systems[0], Is.TypeOf<StructureXml>());
            Assert.That(((StructureXml)result.Systems[0]).Item, Is.TypeOf<LevelXml>());
            Assert.That(result.Constraints[0], Is.TypeOf<ExistsXml>());

        }

        [Test]
        public void Build_ExistsTemplateForTwoDimensions_TwoTestsReturned()
        {
            var template = TEMPLATE;

            var variables = new string[]
            {
                "level.caption",
                "hierarchy.caption",
                "dimension.caption",
                "perspective.caption"
            };

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

            var gtm = new GenericTestMaker(template, variables);

            var results = gtm.Build(info);

            Assert.That(results, Has.Count.EqualTo(2));
        }
    }
}
