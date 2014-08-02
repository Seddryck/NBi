#region Using directives
using System;
using System.Linq;
using Moq;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Builder;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class StructureContainBuilderTest
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

        //@@@@@@@@@@@@@@@@@@@@@@@@@
        //    GetConstraint()
        //@@@@@@@@@@@@@@@@@@@@@@@@@

        [Test]
        public void GetConstraint_BuildUniqueItem_CorrectConstraint()
        {
            //Buiding object used during test
            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            sutXml.Item.Caption = "MeasureGroup";

            var ctrXml = new ContainXml();
            ctrXml.Caption = "Search";

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ContainConstraint>());
        }

        [Test]
        public void GetConstraint_BuildMultipleItem_CorrectConstraint()
        {
            //Buiding object used during test
            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";

            var ctrXml = new ContainXml();
            ctrXml.Items.Add("Search 1");
            ctrXml.Items.Add("Search 2");
            ctrXml.Items.Add("Search 3");

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<ContainConstraint>());
        }



        //@@@@@@@@@@@@@@@@@@@@@@@@@
        //    GetSystemUnderTest()
        //@@@@@@@@@@@@@@@@@@@@@@@@@


        //**********************
        //       Default ConnectionString
        //**********************

        [Test]
        public void GetSystemUnderTest_ConnectionStringInDefault_CorrectlyInitialized()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();

            sutXml.Item = new MeasureGroupsXml();
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";

            sutXml.Default = new DefaultXml() { ConnectionString = "connectionString-default" };

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
            Assert.That(((MetadataDiscoveryRequest)sut).ConnectionString, Is.EqualTo("connectionString-default"));
        }


        //**********************
        //       Pespective
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectPerspectiveTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new PerspectivesXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }

        //**********************
        //       Measure-Group
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectMeasureGroupTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }

        [Test]
        public void GetSystemUnderTest_InCorrectMeasureGroupTargetWithoutCaption_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }

        [Test]
        public void GetSystemUnderTest_IncorrectMeasureGroupTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = "ConnectionString";

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<DiscoveryRequestFactoryException>(delegate { builder.Build(); });
        }

        //**********************
        //       Measure
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectMeasureTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasuresXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasuresXml)sutXml.Item).Perspective = "Perspective";
            ((MeasuresXml)sutXml.Item).MeasureGroup = "MeasureGroup";
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }

        //**********************
        //       Dimension
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectDimensionTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            var dim = new DimensionsXml();
            dim.ConnectionString = "ConnectionString";
            dim.Perspective = "Perspective";
            sutXml.Item = dim;

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }



        [Test]
        public void GetSystemUnderTest_IncorrectDimensionTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionsXml();
            sutXml.Item.ConnectionString = "ConnectionString";

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<DiscoveryRequestFactoryException>(delegate { builder.Build(); });
        }

        //**********************
        //       Hierarchies
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectHierarchyTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((DimensionsXml)sutXml.Item).Perspective = "Perspective";

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }



        [Test]
        public void GetSystemUnderTest_IncorrectHierarchyTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionsXml();
            sutXml.Item.ConnectionString = "ConnectionString";

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<DiscoveryRequestFactoryException>(delegate { builder.Build(); });
        }

        //**********************
        //       Levels
        //**********************


        [Test]
        public void GetSystemUnderTest_CorrectLevelTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new LevelsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((LevelsXml)sutXml.Item).Perspective = "Perspective";
            ((LevelsXml)sutXml.Item).Dimension = "Dimension";
            ((LevelsXml)sutXml.Item).Hierarchy = "Hierarchy";
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }

        [Test]
        public void GetSystemUnderTest_InCorrectLevelTargetWithoutHierarchy_ThrowException()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new LevelsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((LevelsXml)sutXml.Item).Perspective = "Perspective";
            ((LevelsXml)sutXml.Item).Dimension = "Dimension";

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<DiscoveryRequestFactoryException>(delegate { builder.Build(); });
        }

        //**********************
        //       Properties
        //**********************


        [Test]
        public void GetSystemUnderTest_CorrectPropertyTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new PropertiesXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((PropertiesXml)sutXml.Item).Perspective = "Perspective";
            ((PropertiesXml)sutXml.Item).Dimension = "Dimension";
            ((PropertiesXml)sutXml.Item).Hierarchy = "Hierarchy";
            ((PropertiesXml)sutXml.Item).Level = "Level";
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }

        [Test]
        public void GetSystemUnderTest_InCorrectPropertyTargetWithoutLevel_ThrowException()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new PropertiesXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((PropertiesXml)sutXml.Item).Perspective = "Perspective";
            ((PropertiesXml)sutXml.Item).Dimension = "Dimension";
            ((PropertiesXml)sutXml.Item).Hierarchy = "Hierarchy";

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<DiscoveryRequestFactoryException>(delegate { builder.Build(); });
        }
    }
}
