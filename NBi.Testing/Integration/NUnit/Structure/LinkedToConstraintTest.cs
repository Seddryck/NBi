#region Using directives
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Integration.NUnit.Structure
{
    [TestFixture]
    public class LinkedToConstraintTest
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
        public void LinkedToConstraint_ExistingPerspectiveDimensionAndMeasureGroupLinked_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , "Adventure Works", null, "Customer" 
                        );

            var ctr = new LinkedToConstraint("Internet Sales");

            //Method under test
            Assert.That(discovery, ctr);
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveDimensionAndNotExistingMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , "Adventure Works", null, "Customers"
                        );

            var ctr = new LinkedToConstraint("Not existing");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveNotExistingDimensionAndExistingMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , "Adventure Works", null, "Not existing"
                        );

            var ctr = new LinkedToConstraint("Reseller Sales");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_NotExistingPerspectiveExistingDimensionAndExistingMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , "Not existing", null, "Customer"
                        );

            var ctr = new LinkedToConstraint("Internet Sales");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveDimensionAndExistingMeasureGroupButNotLinked_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , "Adventure Works", null, "Customer"
                        );

            var ctr = new LinkedToConstraint("Reseller Sales");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        //Measure-group

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveMeasureGroupAndDimensionLinked_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , "Adventure Works", "Internet Sales", null
                        );

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.That(discovery, ctr);
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveMeasureGroupAndNotExistingDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , "Adventure Works", "Internet Sales", null
                        );

            var ctr = new LinkedToConstraint("Not existing");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveNotExistingMeasureGroupAndExistingDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , "Adventure Works", "Not existing", null
                        );

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_NotExistingPerspectiveExistingMeasureGroupAndExistingDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , "Not existing", "Internet Sales", null
                        );

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveMeasureGroupAndExistingDimensionButNotLinked_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildLinkedTo(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , "Adventure Works","Reseller Sales", null
                        );

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

    }

}
