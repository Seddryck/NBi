#region Using directives
using NBi.Core.Analysis.Discovery;
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
            var discovery = DiscoveryFactory.BuildForCube(
                        ConnectionStringReader.GetAdomd());

            var ctr = new ContainsConstraint("Adventure Works");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingPerspective_Failure()
        {
            var discovery = DiscoveryFactory.BuildForCube(
                        ConnectionStringReader.GetAdomd());

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingDimension_Success()
        {
            var discovery = DiscoveryFactory.BuildForPerspective(
                        ConnectionStringReader.GetAdomd(),
                        "Adventure Works");
            
            var ctr = new ContainsConstraint("Product");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingDimension_Failure()
        {
            var discovery = DiscoveryFactory.BuildForPerspective(
                        ConnectionStringReader.GetAdomd(),
                        "Adventure Works");

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }


        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingHierarchyBellowSpecificDimension_Success()
        {
            var discovery = DiscoveryFactory.BuildForDimension(
                        ConnectionStringReader.GetAdomd(),
                        "Adventure Works",
                        "[Product]");
            
            var ctr = new ContainsConstraint("Product Model Lines");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingHierarchyBellowSpecificDimension_Failure()
        {
            var discovery = DiscoveryFactory.BuildForDimension(
                        ConnectionStringReader.GetAdomd(),
                        "Adventure Works",
                        "[Product]");

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        
        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingLevel_Success()
        {
            var discovery = DiscoveryFactory.BuildForHierarchy(
                        ConnectionStringReader.GetAdomd(),
                        "Adventure Works",
                        "[Customer].[Customer Geography]");
            
            var ctr = new ContainsConstraint("City");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingLevel_Failure()
        {
            var discovery = DiscoveryFactory.BuildForHierarchy(
                        ConnectionStringReader.GetAdomd(),
                        "Adventure Works",
                        "[Customer].[Customer Geography]");

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingMeasureGroup_Success()
        {
            var discovery = DiscoveryFactory.BuildForPerspective(
                        ConnectionStringReader.GetAdomd(),
                        "Adventure Works");

            var ctr = new ContainsConstraint("Reseller Orders");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingMeasureGroup_Failure()
        {
            var discovery = DiscoveryFactory.BuildForPerspective(
                         ConnectionStringReader.GetAdomd(),
                         "Adventure Works");

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingMeasure_Success()
        {
            var discovery = DiscoveryFactory.BuildForMeasureGroup(
                        ConnectionStringReader.GetAdomd(),
                        "Adventure Works",
                        "Reseller Orders");

            var ctr = new ContainsConstraint("Reseller Order Count");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingMeasure_Failure()
        {
            var discovery = DiscoveryFactory.BuildForMeasureGroup(
                        ConnectionStringReader.GetAdomd(),
                        "Adventure Works",
                        "Reseller Orders");

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        

    }

}
