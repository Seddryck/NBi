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
    public class StructureContainsBuilderTest
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
        public void GetConstraint_Build_CorrectConstraint()
        {
            //Buiding object used during test
            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupXml)sutXml.Item).Perspective = "Perspective";
            sutXml.Item.Caption = "MeasureGroup";

            var ctrXml = new ContainsXml();
            ctrXml.Caption = "Search";

            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var ctr = builder.GetConstraint();

            Assert.That(ctr, Is.InstanceOf<CollectionItemConstraint>());
        }


        [Test]
        public void GetConstraint_BuildWithExactlySetToTrue_CorrectConstraint()
        {
            //Buiding object used during test
            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupXml)sutXml.Item).Perspective = "Perspective";
            sutXml.Item.Caption = "MeasureGroup";

            var ctrXml = new ContainsXml();
            ctrXml.Items.Add("Search");
            ctrXml.Items.Add("Search 2");
            ctrXml.Exactly = true;

            var builder = new StructureContainsBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Default = new DefaultXml() { ConnectionString = "connectionString-default" };

            sutXml.Item = new MeasureGroupXml();
            ((MeasureGroupXml)sutXml.Item).Perspective = "Perspective";
            sutXml.Item.Caption = "MeasureGroup";
            var builder = new StructureContainsBuilder();
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
        public void GetSystemUnderTest_CorrectPerspectiveTarget_FailBecausePerspectiveDoesNotSupportContains()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new PerspectiveXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            sutXml.Item.Caption = "Perspective";
            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }
        
        //**********************
        //       Measure-Group
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectMeasureGroupTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupXml)sutXml.Item).Perspective = "Perspective";
            sutXml.Item.Caption = "MeasureGroup";
            var builder = new StructureContainsBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((MeasureGroupXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }
        
        [Test]
        public void GetSystemUnderTest_IncorrectMeasureGroupTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            sutXml.Item.Caption = "MeasureGroup";

            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }

        //**********************
        //       Measure
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectMeasureTarget_FailBecauseMeasureDoesNotSupportContains()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            sutXml.Item.Caption = "Measure";
            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }

        //**********************
        //       Dimension
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectDimensionTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            var dim = new DimensionXml();
            dim.ConnectionString = "ConnectionString";
            dim.Perspective = "Perspective";
            dim.Caption = "dimension";
            sutXml.Item = dim;

            var builder = new StructureContainsBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionXml();
            sutXml.Item.ConnectionString = "ConnectionString";

            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build();});
        }

        //**********************
        //       Hierarchies
        //**********************

        [Test]
        public void GetSystemUnderTest_CorrectHierarchyTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((DimensionXml)sutXml.Item).Perspective = "Perspective";
            sutXml.Item.Caption = "dimension";

            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<MetadataDiscoveryRequest>());
        }

        [Test]
        public void GetSystemUnderTest_IncorrectHierarchyTargetWithoutCaptionSpecified_ThrowException()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            ((DimensionXml)sutXml.Item).Perspective = "Perspective";

            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }

        [Test]
        public void GetSystemUnderTest_IncorrectHierarchyTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new DimensionXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            sutXml.Item.Caption = "dimension";

            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }

        //**********************
        //       Levels
        //**********************


        [Test]
        public void GetSystemUnderTest_CorrectLevelTarget_FailBecauseLevelDoesNotSupportContains()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<ContainsXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new LevelXml();
            sutXml.Item.ConnectionString = "ConnectionString";
            sutXml.Item.Caption = "Level";
            ((LevelXml)sutXml.Item).Dimension = "Dimension";
            ((LevelXml)sutXml.Item).Hierarchy = "Hierarchy";
            var builder = new StructureContainsBuilder();
            builder.Setup(sutXml, ctrXml);
            //Assertion
            Assert.Throws<ArgumentException>(delegate { builder.Build(); });
        }

        //[Ignore]
        //[Test]
        //public void GetSystemUnderTest_CorrectLevelTarget_Success()
        //{
        //    //Buiding object used during test
        //    var xml = new LevelXml();
        //    xml.Perspective = "Perspective";
        //    xml.ConnectionString = "ConnectionString";
        //    xml.Dimension = "dimension";
        //    xml.Hierarchy = "hierarchy";
        //    xml.Caption = "level";
            
        //    //Call the method to test
        //    var actual = xml.GetSystemUnderTest_();

        //    //Assertion
        //    Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        //}

        //[Ignore]
        //[Test]
        //public void GetSystemUnderTest_IncorrectLevelTargetWithoutPathSpecified_ThrowException()
        //{
        //    //Buiding object used during test
        //    var xml = new LevelXml();
        //    xml.Perspective = "Perspective";
        //    xml.ConnectionString = "ConnectionString";

        //    //Assertion
        //    Assert.Throws<DiscoveryFactoryException>(delegate { xml.GetSystemUnderTest_(); });
        //}

        //[Ignore]
        //[Test]
        //public void GetSystemUnderTest_IncorrectLevelTargetWithoutPerspective_ThrowException()
        //{
        //    //Buiding object used during test
        //    var xml = new LevelXml();
        //    xml.Dimension = "dimension";
        //    xml.Hierarchy = "hierarchy";
        //    xml.ConnectionString = "ConnectionString";
            
        //    //Assertion
        //    Assert.Throws<DiscoveryFactoryException>(delegate { xml.GetSystemUnderTest_(); });
        //}

    }
}
