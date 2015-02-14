#region Using directives
using System.Collections.Generic;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Integration.NUnit.Structure
{
    [TestFixture]
    [Category("Olap")]
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
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Customer", DiscoveryTarget.Dimensions)
                        });

            var ctr = new LinkedToConstraint("Internet Sales");

            //Method under test
            Assert.That(discovery, ctr);
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveDimensionAndMeasureGroupLinkedWithoutCaseMatching_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Customer", DiscoveryTarget.Dimensions)
                        });

            var ctr = new LinkedToConstraint("Internet Sales".ToLower());
            ctr = ctr.IgnoreCase;

            //Method under test
            Assert.That(discovery, ctr);
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveDimensionAndNotExistingMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Customer", DiscoveryTarget.Dimensions)
                        });

            var ctr = new LinkedToConstraint("Not existing");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveNotExistingDimensionAndExistingMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Not existing", DiscoveryTarget.Dimensions)
                        });

            var ctr = new LinkedToConstraint("Reseller Sales");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_NotExistingPerspectiveExistingDimensionAndExistingMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>() { 
                            new CaptionFilter("Not existing", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Customer", DiscoveryTarget.Dimensions)
                        });

            var ctr = new LinkedToConstraint("Internet Sales");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveDimensionAndExistingMeasureGroupButNotLinked_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Customer", DiscoveryTarget.Dimensions)
                        });

            var ctr = new LinkedToConstraint("Reseller Sales");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        //Measure-group

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveMeasureGroupAndDimensionLinked_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Internet Sales", DiscoveryTarget.MeasureGroups)
                        });

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.That(discovery, ctr);
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveMeasureGroupAndNotExistingDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Internet Sales", DiscoveryTarget.MeasureGroups)
                        });

            var ctr = new LinkedToConstraint("Not existing");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveNotExistingMeasureGroupAndExistingDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Not existing", DiscoveryTarget.MeasureGroups)
                        });

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_NotExistingPerspectiveExistingMeasureGroupAndExistingDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , new List<IFilter>() { 
                            new CaptionFilter("Not existing", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Internet Sales", DiscoveryTarget.MeasureGroups)
                        });

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveMeasureGroupAndExistingDimensionButNotLinked_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildRelation(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                            , new CaptionFilter("Reseller Sales", DiscoveryTarget.MeasureGroups)
                        });

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

    }

}
