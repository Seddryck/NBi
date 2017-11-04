#region Using directives
using NBi.Core.Structure;
using NBi.NUnit.Builder;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Systems;
using NUnit.Framework;
using System;
using System.Collections;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class StructureExistsBuilderTest
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
        public void GetConstraint_Build_CorrectConstraint()
        {
            var sutXml = new StructureXml()
            {
                Item = new HierarchyXml()
                {
                    ConnectionString = ConnectionStringReader.GetAdomd(),
                    Perspective = "perspective-name",
                    Dimension = "dimension-caption",
                    Caption = "hierarchy-caption",
                }
            };
            var ctrXml = new ExistsXml();

            var builder = new StructureExistsBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ExistsConstraint>());
        }

        [Test]
        public void GetSystemUnderTest_Build_CorrectSystemUnderTest()
        {
            var sutXml = new StructureXml();
            var item = new PerspectiveXml();
            sutXml.Item = item;
            item.ConnectionString = ConnectionStringReader.GetAdomd();
            item.Caption = "perspective";
            var ctrXml = new ExistsXml();

            var builder = new StructureExistsBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            Assert.That(sut, Is.InstanceOf<StructureDiscoveryCommand>());
        }

        [Test]
        public void GetConstraint_BuildWithIgnoreCase_ComparerCaseInsensitive()
        {
            var sutXml = new StructureXml();
            var item = new PerspectiveXml();
            sutXml.Item = item;
            item.ConnectionString = ConnectionStringReader.GetAdomd();
            item.Caption = "perspective";
            var ctrXml = new ExistsXml() { IgnoreCase = true };

            var builder = new StructureExistsBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            var existsCtr = (ExistsConstraint)ctr;
            Assert.That(existsCtr.Comparer, Is.InstanceOf<CaseInsensitiveComparer>());
            Assert.That(existsCtr.Comparer.Compare("c", "C"), Is.EqualTo(0));
        }

        [Test]
        public void GetConstraint_BuildWithoutIgnoreCase_ComparerCaseSensitive()
        {
            var sutXml = new StructureXml();
            var item = new PerspectiveXml();
            sutXml.Item = item;
            item.ConnectionString = ConnectionStringReader.GetAdomd();
            item.Caption = "perspective";
            var ctrXml = new ExistsXml();

            var builder = new StructureExistsBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            var existsCtr = (ExistsConstraint)ctr;

            Assert.That(existsCtr.Comparer, Is.InstanceOf<Comparer>());
            Assert.That(existsCtr.Comparer.Compare("c", "C"), Is.Not.EqualTo(0));
        }

    }
}
