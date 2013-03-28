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
    public class StructureEquivalentToBuilderTest
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
        public void GetConstraint_BuildWithWithList_CorrectConstraint()
        {
            //Buiding object used during test
            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";

            var ctrXml = new EquivalentToXml();
            ctrXml.Items.Add("Search");
            ctrXml.Items.Add("Search 2");

            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<EquivalentToConstraint>());
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
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Default = new DefaultXml() { ConnectionString = "connectionString-default" };

            sutXml.Item = new MeasureGroupsXml();
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureEquivalentToBuilder();
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
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new PerspectivesXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            var builder = new StructureEquivalentToBuilder();
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
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureEquivalentToBuilder();
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
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureEquivalentToBuilder();
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
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = "ConnectionString";

            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }

        //**********************
        //       Measure
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectMeasureTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasuresXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasuresXml)sutXml.Item).Perspective = "Perspective";
            ((MeasuresXml)sutXml.Item).MeasureGroup = "MeasureGroup";
            var builder = new StructureEquivalentToBuilder();
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
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            var dim = new DimensionsXml();
            dim.ConnectionString = "ConnectionString";
            dim.Perspective = "Perspective";
            sutXml.Item = dim;

            var builder = new StructureEquivalentToBuilder();
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
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionsXml();
            sutXml.Item.ConnectionString = "ConnectionString";

            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }

        //**********************
        //       Hierarchies
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectHierarchyTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((DimensionsXml)sutXml.Item).Perspective = "Perspective";

            var builder = new StructureEquivalentToBuilder();
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
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionsXml();
            sutXml.Item.ConnectionString = "ConnectionString";

            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }

        //**********************
        //       Levels
        //**********************


        [Test]
        public void GetSystemUnderTest_CorrectLevelTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new LevelsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((LevelsXml)sutXml.Item).Perspective = "Perspective";
            ((LevelsXml)sutXml.Item).Dimension = "Dimension";
            ((LevelsXml)sutXml.Item).Hierarchy = "Hierarchy";
            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }

        [Test]
        public void GetSystemUnderTest_InCorrectLevelTargetWithoutHGierarchy_ThrowException()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new LevelsXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((LevelsXml)sutXml.Item).Perspective = "Perspective";
            ((LevelsXml)sutXml.Item).Dimension = "Dimension";

            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }

    }
}
