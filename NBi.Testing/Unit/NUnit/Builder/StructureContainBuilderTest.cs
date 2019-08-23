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
using NBi.Core.Structure.Olap;
using System.Collections.Generic;
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
            var sutXml = new StructureXml
            {
                Item = new MeasureGroupsXml
                {
                    ConnectionString = ConnectionStringReader.GetAdomd()
                }
            };
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            sutXml.Item.Caption = "MeasureGroup";

            var ctrXml = new ContainXml
            {
                Items = new List<string> { "Search" }
            };

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
            var sutXml = new StructureXml
            {
                Item = new MeasureGroupsXml
                {
                    ConnectionString = ConnectionStringReader.GetAdomd()
                }
            };
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

            var builder = new StructureContainBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml
            {
                Item = new PerspectivesXml
                {
                    ConnectionString = ConnectionStringReader.GetAdomd()
                }
            };
            var builder = new StructureContainBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml
            {
                Item = new MeasureGroupsXml
                {
                    ConnectionString = ConnectionStringReader.GetAdomd()
                }
            };
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureContainBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml
            {
                Item = new MeasureGroupsXml
                {
                    ConnectionString = ConnectionStringReader.GetAdomd()
                }
            };
            ((MeasureGroupsXml)sutXml.Item).Perspective = "Perspective";
            var builder = new StructureContainBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml
            {
                Item = new MeasuresXml
                {
                    ConnectionString = ConnectionStringReader.GetAdomd()
                }
            };
            ((MeasuresXml)sutXml.Item).Perspective = "Perspective";
            ((MeasuresXml)sutXml.Item).MeasureGroup = "MeasureGroup";
            var builder = new StructureContainBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml();
            var dim = new DimensionsXml
            {
                ConnectionString = ConnectionStringReader.GetAdomd(),
                Perspective = "Perspective"
            };
            sutXml.Item = dim;

            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            //Call the method to test
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
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

            var sutXml = new StructureXml
            {
                Item = new DimensionsXml
                {
                    ConnectionString = ConnectionStringReader.GetAdomd()
                }
            };
            ((DimensionsXml)sutXml.Item).Perspective = "Perspective";

            var builder = new StructureContainBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml
            {
                Item = new LevelsXml
                {
                    ConnectionString = ConnectionStringReader.GetAdomd()
                }
            };
            ((LevelsXml)sutXml.Item).Perspective = "Perspective";
            ((LevelsXml)sutXml.Item).Dimension = "Dimension";
            ((LevelsXml)sutXml.Item).Hierarchy = "Hierarchy";
            var builder = new StructureContainBuilder();
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
            var ctrXmlStubFactory = new Mock<ContainXml>();
            var ctrXml = ctrXmlStubFactory.Object;

            var sutXml = new StructureXml
            {
                Item = new PropertiesXml
                {
                    ConnectionString = ConnectionStringReader.GetAdomd()
                }
            };
            ((PropertiesXml)sutXml.Item).Perspective = "Perspective";
            ((PropertiesXml)sutXml.Item).Dimension = "Dimension";
            ((PropertiesXml)sutXml.Item).Hierarchy = "Hierarchy";
            ((PropertiesXml)sutXml.Item).Level = "Level";
            var builder = new StructureContainBuilder();
            builder.Setup(sutXml, ctrXml);
            builder.Build();
            var sut = builder.GetSystemUnderTest();

            //Assertion
            Assert.That(sut, Is.InstanceOf<OlapCommand>());
        }

        
    }
}
