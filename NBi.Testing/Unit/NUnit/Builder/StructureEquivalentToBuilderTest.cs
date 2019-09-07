#region Using directives

using System.Linq;
using Moq;
using NBi.NUnit.Builder;
using NBi.NUnit.Structure;
using NBi.Xml.Constraints;
using NBi.Xml.Items;
using NBi.Xml.Settings;
using NBi.Xml.Systems;
using NUnit.Framework;
using NBi.Core.Structure.Olap;
using NBi.Xml.Items.Filters;
using NBi.Core.Structure;
using System.Collections.Generic;

#endregion

namespace NBi.Testing.Unit.NUnit.Builder
{
    [TestFixture]
    public class StructureEquivalentToBuilderTest
    {

        #region SetUp & TearDown
        //Called only at instance creation
        [OneTimeSetUp]
        public void SetupMethods()
        {

        }

        //Called only at instance destruction
        [OneTimeTearDown]
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
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
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

            var sutXml = new StructureXml()
            {
                Item = new MeasureGroupsXml()
                {
                    Perspective = "Perspective",
                    Settings = new SettingsXml()
                    {
                        Defaults = new List<DefaultXml>()
                        { new DefaultXml() { ConnectionString = new ConnectionStringXml() { Inline = ConnectionStringReader.GetAdomd() } } }
                    }
                }
            };

            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
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
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
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
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }

        [Test]
        public void GetSystemUnderTest_InCorrectMeasureGroupTargetWithoutCaption_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasureGroupsXml();
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
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
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
            ((MeasuresXml)sutXml.Item).Perspective = "Perspective";
            ((MeasuresXml)sutXml.Item).MeasureGroup = "MeasureGroup";
            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
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
            dim.ConnectionString = ConnectionStringReader.GetAdomd();
            dim.Perspective = "Perspective";
            sutXml.Item = dim;

            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            var command = sut as OlapCommand;
            Assert.NotNull(command);
            Assert.NotNull(command.Description);
            Assert.IsTrue(command.Description.Filters.Any(f => f is CaptionFilter && (f as CaptionFilter).Caption == "Perspective"));
            Assert.AreEqual(command.Description.Filters.Count(), 1);
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
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
            ((DimensionsXml)sutXml.Item).Perspective = "Perspective";

            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
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
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
            ((LevelsXml)sutXml.Item).Perspective = "Perspective";
            ((LevelsXml)sutXml.Item).Dimension = "Dimension";
            ((LevelsXml)sutXml.Item).Hierarchy = "Hierarchy";
            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }

        //**********************
        //       Properties
        //**********************


        [Test]
        public void GetSystemUnderTest_CorrectPropertyTarget_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new PropertiesXml();
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
            ((PropertiesXml)sutXml.Item).Perspective = "Perspective";
            ((PropertiesXml)sutXml.Item).Dimension = "Dimension";
            ((PropertiesXml)sutXml.Item).Hierarchy = "Hierarchy";
            ((PropertiesXml)sutXml.Item).Level = "Level";
            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }

        //**********************
        //  Test Filters
        //    IPerspectiveFilter
        //    IMeasureGroupFilter      
        //    IDisplayFolderFilter
        //**********************


        [Test]
        public void GetSystemUndeTest_CorrectFiltersAppliedOnDescriptionPart1_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new MeasuresXml();
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
            ((IPerspectiveFilter)sutXml.Item).Perspective = "Perspective";
            ((IMeasureGroupFilter)sutXml.Item).MeasureGroup = "MeasureGroup";
            ((IDisplayFolderFilter)sutXml.Item).DisplayFolder = "DisplayFolder";
            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());

            var command = sut as OlapCommand;
            Assert.NotNull(command);
            Assert.NotNull(command.Description);
            Assert.IsTrue(command.Description.Filters.Any(f => f is CaptionFilter && (f as CaptionFilter).Caption == "Perspective"));
            Assert.IsTrue(command.Description.Filters.Any(f => f is CaptionFilter && (f as CaptionFilter).Caption == "MeasureGroup"));
            Assert.IsTrue(command.Description.Filters.Any(f => f is CaptionFilter && (f as CaptionFilter).Caption == "DisplayFolder"));
        }


        //**********************
        //  Test Filters
        //    IDimensionFilter
        //    IHierarchyFilter
        //    ILevelFilter
        //**********************


        [Test]
        public void GetSystemUndeTest_CorrectFiltersAppliedOnDescriptionPart2_Success()
        {
            //Buiding object used during test
            var ctrXmlStubFactory = new Mock<EquivalentToXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            sutXml.Item = new PropertiesXml();
            sutXml.Item.ConnectionString = ConnectionStringReader.GetAdomd();
            ((IDimensionFilter)sutXml.Item).Dimension = "Dimension";
            ((IHierarchyFilter)sutXml.Item).Hierarchy = "Hierarchy";
            ((ILevelFilter)sutXml.Item).Level = "Level";
            var builder = new StructureEquivalentToBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());

            var command = sut as OlapCommand;
            Assert.NotNull(command);
            Assert.NotNull(command.Description);
            Assert.IsTrue(command.Description.Filters.Any(f => f is CaptionFilter && (f as CaptionFilter).Caption == "Dimension"));
            Assert.IsTrue(command.Description.Filters.Any(f => f is CaptionFilter && (f as CaptionFilter).Caption == "Hierarchy"));
            Assert.IsTrue(command.Description.Filters.Any(f => f is CaptionFilter && (f as CaptionFilter).Caption == "Level"));
        }
    }
}
