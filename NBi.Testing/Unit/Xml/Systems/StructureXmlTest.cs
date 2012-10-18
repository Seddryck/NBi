#region Using directives
using System;
using System.Linq;
using NBi.Core.Analysis;
using NBi.Core.Analysis.Discovery;
using NBi.Xml.Systems;
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

        [Test]
        public void Instantiate_CorrectPerspectiveTarget_Success()
        {
            //Buiding object used during test
            var xml = new StructureXml();
            xml.Target=DiscoverTarget.Perspectives;
            xml.ConnectionString = "ConnectionString";

            //Call the method to test
            var actual = xml.Instantiate();

            //Assertion
            Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }
        
        //**********************
        //       Measure-Group
        //**********************

        [Test]
        public void Instantiate_CorrectMeasureGroupTarget_Success()
        {
            //Buiding object used during test
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.MeasureGroups;
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
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.MeasureGroups;
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
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Measures;
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
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Measures;
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "Perspective";
            
            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        [Test]
        public void Instantiate_IncorrectMeasureTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Measures;
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
            var xml = new StructureXml();
            xml.ConnectionString = "ConnectionString";
            xml.Target = DiscoverTarget.Dimensions;
            xml.Perspective = "Perspective";

            //Call the method to test
            var actual = xml.Instantiate();

            //Assertion
            Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }


        [Test]
        public void Instantiate_IncorrectDimensionTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Dimensions;
            xml.ConnectionString = "ConnectionString";

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        //**********************
        //       Hierarchies
        //**********************

        [Test]
        public void Instantiate_CorrectHierarchyTarget_Success()
        {
            //Buiding object used during test
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Hierarchies;
            xml.ConnectionString = "ConnectionString";
            xml.Perspective = "Perspective";
            xml.Path = "[dimension]";

            //Call the method to test
            var actual = xml.Instantiate();

            //Assertion
            Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }

        [Test]
        public void Instantiate_IncorrectHierarchyTargetWithoutPathSpecified_ThrowException()
        {
            //Buiding object used during test
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Hierarchies;
            xml.Perspective = "Perspective";
            xml.ConnectionString = "ConnectionString";

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        [Test]
        public void Instantiate_IncorrectHierarchyTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Hierarchies;
            xml.ConnectionString = "ConnectionString";
            xml.Path = "[dimension]";

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
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Levels;
            xml.Perspective = "Perspective";
            xml.ConnectionString = "ConnectionString";
            xml.Path = "[dimension].[hierarchy]";

            //Call the method to test
            var actual = xml.Instantiate();

            //Assertion
            Assert.That(actual, Is.InstanceOfType<DiscoveryCommand>());
        }

        [Test]
        public void Instantiate_IncorrectLevelTargetWithoutPathSpecified_ThrowException()
        {
            //Buiding object used during test
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Levels;
            xml.Perspective = "Perspective";
            xml.ConnectionString = "ConnectionString";

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

        [Test]
        public void Instantiate_IncorrectLevelTargetWithoutPerspective_ThrowException()
        {
            //Buiding object used during test
            var xml = new StructureXml();
            xml.Target = DiscoverTarget.Levels;
            xml.Path = "[dimension].[hierarchy]";
            xml.ConnectionString = "ConnectionString";

            //Assertion
            Assert.Throws<DiscoveryFactoryException>(delegate { xml.Instantiate(); });
        }

    }
}
