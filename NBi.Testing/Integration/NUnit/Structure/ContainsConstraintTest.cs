#region Using directives
using NBi.Core.Analysis;
using NBi.Core.Analysis.Metadata;
using NBi.NUnit.Structure;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Integration.NUnit.Structure
{
    [TestFixture]
    public class ContainsConstraintTest
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

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingPerspective_Success()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Perspectives, ConnectionStringReader.GetAdomd());

            var ctr = new ContainsConstraint("Adventure Works");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingPerspective_Failure()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Perspectives, ConnectionStringReader.GetAdomd());

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingDimension_Success()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Dimensions, ConnectionStringReader.GetAdomd());
            discovery.Perspective = "Adventure Works";

            var ctr = new ContainsConstraint("Product");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingDimension_Failure()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Dimensions, ConnectionStringReader.GetAdomd());
            discovery.Perspective = "Adventure Works";

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }


        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingHierarchyBellowSpecificDimension_Success()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Hierarchies, ConnectionStringReader.GetAdomd());
            discovery.Path = "[Product]";
            discovery.Perspective = "Adventure Works";
            
            var ctr = new ContainsConstraint("Product Model Lines");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingHierarchyBellowSpecificDimension_Failure()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Hierarchies, ConnectionStringReader.GetAdomd());
            discovery.Path = "[Product]";
            discovery.Perspective = "Adventure Works";

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        
        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingLevel_Success()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Levels, ConnectionStringReader.GetAdomd());
            discovery.Path = "[Customer].[Customer Geography]";
            discovery.Perspective = "Adventure Works";
            
            var ctr = new ContainsConstraint("City");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingLevel_Failure()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Levels, ConnectionStringReader.GetAdomd());
            discovery.Path = "[Customer].[Customer Geography]";
            discovery.Perspective = "Adventure Works";

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingMeasureGroup_Success()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.MeasureGroups, ConnectionStringReader.GetAdomd());
            discovery.Perspective = "Adventure Works";

            var ctr = new ContainsConstraint("Reseller Orders");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingMeasureGroup_Failure()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.MeasureGroups, ConnectionStringReader.GetAdomd());
            discovery.Perspective = "Adventure Works";

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingMeasure_Success()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Measures, ConnectionStringReader.GetAdomd());
            discovery.Perspective = "Adventure Works";
            discovery.MeasureGroup = "Reseller Orders";

            var ctr = new ContainsConstraint("Reseller Order Count");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingMeasureBellowSpecificDimension_Failure()
        {
            var discovery = new DiscoverCommand(DiscoverTarget.Measures, ConnectionStringReader.GetAdomd());
            discovery.Perspective = "Adventure Works";
            discovery.MeasureGroup = "Reseller Orders";

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        

    }

}
