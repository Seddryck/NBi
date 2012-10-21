#region Using directives
using System;
using System.Linq;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Discovery;
using NBi.Xml.Systems;
using NBi.Xml.Systems.Structure;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Xml.Systems
{
    [TestFixture]
    public class StructureXmlTest
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

        [Ignore]
        [Test]
        public void Instantiate_CorrectPerspectiveTarget_Success()
        {
            //Buiding object used during test
            //var xml = new CubeXml();
            //xml.ConnectionString = "ConnectionString";

            //Call the method to test
            //var actual = xml.Instantiate();

            //Assertion
            //Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }
        
        //**********************
        //       Measure-Group
        //**********************

        [Test]
        public void Instantiate_CorrectMeasureGroupTarget_Success()
        {
            //Buiding object used during test
            var xml = new MeasureGroupXml();
            xml.Perspective = "Perspective";
            xml.ConnectionString = "ConnectionString";

            //Call the method to test
            var actual = xml.Instantiate();

            //Assertion
            Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }
        
        [Test]
        public void Instantiate_IncorrectMeasureGroupTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var xml = new MeasureGroupXml();
            xml.ConnectionString = "ConnectionString";

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        //**********************
        //       Measure
        //**********************

        [Test]
        public void Instantiate_CorrectMeasureTarget_Success()
        {
            //Buiding object used during test
            var xml = new MeasureXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "Perspective";
            xml.MeasureGroup = "MeasureGroup";

            //Call the method to test
            var actual = xml.Instantiate();

            //Assertion
            Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }

        [Test]
        public void Instantiate_CorrectMeasureTargetWithoutMeasureGroup_ThrowException()
        {
            //Buiding object used during test
            var xml = new MeasureXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "Perspective";
            
            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        [Test]
        public void Instantiate_IncorrectMeasureTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var xml = new MeasureXml();
            xml.ConnectionString = "ConnectionString";
            xml.MeasureGroup = "measure-group";

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        //**********************
        //       Dimension
        //**********************

        [Test]
        public void Instantiate_CorrectDimensionTarget_Success()
        {
            //Buiding object used during test
            var xml = new DimensionXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "Perspective";
            xml.Structure = new StructureXml();

            //Call the method to test
            var actual = xml.Instantiate();

            //Assertion
            Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }



        [Test]
        public void Instantiate_IncorrectDimensionTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var xml = new DimensionXml();
            xml.ConnectionString = "ConnectionString";

            //Assertion
            Assert.Throws<ArgumentNullException>(delegate { xml.Instantiate(); });
        }

        //**********************
        //       Hierarchies
        //**********************

        [Test]
        public void Instantiate_CorrectHierarchyTarget_Success()
        {
            //Buiding object used during test
            var xml = new DimensionXml();
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "Perspective";
            xml.Caption = "dimension";
            xml.Structure = new StructureXml();

            //Call the method to test
            var actual = xml.Instantiate();

            //Assertion
            Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }

        [Test]
        public void Instantiate_IncorrectHierarchyTargetWithoutCaptionSpecified_ThrowException()
        {
            //Buiding object used during test
            var xml = new DimensionXml();
            xml.ConnectionString = "ConnectionString";
            xml.Structure = new StructureXml();

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        [Test]
        public void Instantiate_IncorrectHierarchyTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var xml = new DimensionXml();
            xml.ConnectionString = "ConnectionString";
            xml.Caption = "dimension";
            xml.Structure = new StructureXml();

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        //**********************
        //       Levels
        //**********************

        [Test]
        public void Instantiate_CorrectLevelTarget_Success()
        {
            //Buiding object used during test
            var xml = new LevelXml();
            xml.Perspective = "Perspective";
            xml.ConnectionString = "ConnectionString";
            xml.Dimension = "dimension";
            xml.Hierarchy = "hierarchy";
            xml.Caption = "level";
            xml.Structure = new StructureXml();

            //Call the method to test
            var actual = xml.Instantiate();

            //Assertion
            Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }

        [Ignore]
        [Test]
        public void Instantiate_IncorrectLevelTargetWithoutPathSpecified_ThrowException()
        {
            //Buiding object used during test
            var xml = new LevelXml();
            xml.Perspective = "Perspective";
            xml.ConnectionString = "ConnectionString";

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        [Test]
        public void Instantiate_IncorrectLevelTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var xml = new LevelXml();
            xml.Dimension = "dimension";
            xml.Hierarchy = "hierarchy";
            xml.ConnectionString = "ConnectionString";
            xml.Structure = new StructureXml();

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

    }
}
