#region Using directives
using System.Collections.Generic;
using NBi.Core.Analysis.Request;
using NBi.NUnit.Structure;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Integration.NUnit.Structure
{
    [TestFixture]
    [Category ("Olap")]
    public class ContainConstraintTest
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
        public void Matches_ExpectedContainedInActual_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , new List<IFilter>());

            var ctr = new ContainConstraint("Adventure Works");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void Matches_ExpectedContainedInActualCaseNotMatching_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , new List<IFilter>());

            var ctr = new ContainConstraint("Adventure Works".ToLower());
            ctr = ctr.IgnoreCase;

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void Matches_ExpectedNotContainedInActual_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , new List<IFilter>());

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

       ///Dimension !!!!

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingDimension_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                        });

                        

            var ctr = new ContainConstraint("Product");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }


        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingHierarchyBellowSpecificDimension_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Hierarchies
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("Product", DiscoveryTarget.Dimensions)
                        });


            var ctr = new ContainConstraint("Product Model Lines");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingHierarchyBellowSpecificDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Hierarchies
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("Product", DiscoveryTarget.Dimensions)
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }


        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingLevel_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Levels
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("Customer", DiscoveryTarget.Dimensions)
                                , new CaptionFilter("Customer Geography", DiscoveryTarget.Hierarchies)
                        });

            var ctr = new ContainConstraint("City");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingLevel_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Levels
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("Customer", DiscoveryTarget.Dimensions)
                                , new CaptionFilter("Customer Geography", DiscoveryTarget.Hierarchies)
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingMeasureGroup_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                        });

            var ctr = new ContainConstraint("Reseller Orders");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingMeasure_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Measures
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("Reseller Orders", DiscoveryTarget.MeasureGroups)
                        });

            var ctr = new ContainConstraint("Reseller Order Count");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingMeasure_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                         ConnectionStringReader.GetAdomd()
                         , DiscoveryTarget.Measures
                         , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("Reseller Orders", DiscoveryTarget.MeasureGroups)
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingMeasureWithoutMeasureGroup_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Measures
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                        });

            var ctr = new ContainConstraint("Reseller Order Count");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingMeasureWithoutMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                         ConnectionStringReader.GetAdomd()
                         , DiscoveryTarget.Measures
                         , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingMeasureWithCaseNonMatching_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Measures
                        , new List<IFilter>()
                            {
                                new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives)
                                , new CaptionFilter("Reseller Orders", DiscoveryTarget.MeasureGroups)
                        });

            var ctr = new ContainConstraint("Reseller Order Count".ToLower());
            ctr = ctr.IgnoreCase;

            //Method under test
            Assert.That(discovery, ctr);

        }
    }

}
