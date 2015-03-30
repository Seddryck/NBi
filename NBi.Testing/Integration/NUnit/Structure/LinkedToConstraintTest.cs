#region Using directives
using System.Collections.Generic;
using NBi.NUnit.Structure;
using NUnit.Framework;
using NBi.Core.Structure;
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
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.MeasureGroups
                        , TargetType.Relation
                        , new CaptionFilter[] { 
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.Dimensions, "Customer")
                        });

            var ctr = new LinkedToConstraint("Internet Sales");

            //Method under test
            Assert.That(discovery, ctr);
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveDimensionAndMeasureGroupLinkedWithoutCaseMatching_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.MeasureGroups
                        , TargetType.Relation
                        , new CaptionFilter[] {
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.Dimensions, "Customer")
                        });

            var ctr = new LinkedToConstraint("Internet Sales".ToLower());
            ctr = ctr.IgnoreCase;

            //Method under test
            Assert.That(discovery, ctr);
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveDimensionAndNotExistingMeasureGroup_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.MeasureGroups
                        , TargetType.Relation
                        , new CaptionFilter[] {
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.Dimensions, "Customer")
                        });

            var ctr = new LinkedToConstraint("Not existing");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveNotExistingDimensionAndExistingMeasureGroup_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.MeasureGroups
                        , TargetType.Relation
                        , new CaptionFilter[] {
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.Dimensions, "Not existing")
                        });

            var ctr = new LinkedToConstraint("Reseller Sales");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_NotExistingPerspectiveExistingDimensionAndExistingMeasureGroup_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.MeasureGroups
                        , TargetType.Relation
                        , new CaptionFilter[] {
                            new CaptionFilter(Target.Perspectives, "Not existing")
                            , new CaptionFilter(Target.Dimensions, "Customer")
                        });

            var ctr = new LinkedToConstraint("Internet Sales");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveDimensionAndExistingMeasureGroupButNotLinked_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.MeasureGroups
                        , TargetType.Relation
                        , new CaptionFilter[] {
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.Dimensions, "Customer")
                        });

            var ctr = new LinkedToConstraint("Reseller Sales");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        //Measure-group

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveMeasureGroupAndDimensionLinked_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Dimensions
                        , TargetType.Relation
                        , new CaptionFilter[] {
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.MeasureGroups, "Internet Sales")
                        });

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.That(discovery, ctr);
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveMeasureGroupAndNotExistingDimension_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Dimensions
                        , TargetType.Relation
                        , new CaptionFilter[] { 
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.MeasureGroups, "Internet Sales")
                        });

            var ctr = new LinkedToConstraint("Not existing");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveNotExistingMeasureGroupAndExistingDimension_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Dimensions
                        , TargetType.Relation
                        , new CaptionFilter[] { 
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.MeasureGroups, "Not existing")
                        });

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_NotExistingPerspectiveExistingMeasureGroupAndExistingDimension_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Dimensions
                        , TargetType.Relation
                        , new CaptionFilter[] {
                            new CaptionFilter(Target.Perspectives, "Not existing")
                            , new CaptionFilter(Target.MeasureGroups, "Internet Sales")
                        });

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void LinkedToConstraint_ExistingPerspectiveMeasureGroupAndExistingDimensionButNotLinked_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Dimensions
                        , TargetType.Relation
                        , new CaptionFilter[] {
                            new CaptionFilter(Target.Perspectives, "Adventure Works")
                            , new CaptionFilter(Target.MeasureGroups, "Reseller Sales")
                        });

            var ctr = new LinkedToConstraint("Customer");

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

    }

}
