#region Using directives
using System.Collections.Generic;
using NBi.NUnit.Structure;
using NUnit.Framework;
using NBi.Core.Structure;
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
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Perspectives
                        , TargetType.Object
                        , new CaptionFilter[] { });

            var ctr = new ContainConstraint("Adventure Works");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void Matches_ExpectedContainedInActualCaseNotMatching_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Perspectives
                        , TargetType.Object
                        , new CaptionFilter[] { });

            var ctr = new ContainConstraint("Adventure Works".ToLower());
            ctr = ctr.IgnoreCase;

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void Matches_ExpectedNotContainedInActual_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Perspectives
                        , TargetType.Object
                        , new CaptionFilter[] { });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

       ///Dimension !!!!

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingDimension_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Dimensions
                        , TargetType.Object
                        , new CaptionFilter[] {new CaptionFilter(Target.Perspectives, "Adventure Works")}
                        );

            var ctr = new ContainConstraint("Product");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingDimension_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Dimensions
                        , TargetType.Object
                        , new CaptionFilter[] { new CaptionFilter(Target.Perspectives, "Adventure Works") }
                        );

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }


        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingHierarchyBellowSpecificDimension_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Hierarchies
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                                , new CaptionFilter(Target.Dimensions, "Product")
                        });

            var ctr = new ContainConstraint("Product Model Lines");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingHierarchyBellowSpecificDimension_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Hierarchies
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                                , new CaptionFilter(Target.Dimensions, "Product")
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }


        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingLevel_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Levels
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                                , new CaptionFilter(Target.Dimensions, "Customer")
                                , new CaptionFilter(Target.Hierarchies, "Customer Geography")
                        });

            var ctr = new ContainConstraint("City");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingLevel_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Levels
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                                , new CaptionFilter(Target.Dimensions, "Product")
                                , new CaptionFilter(Target.Hierarchies, "Customer Geography")
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingMeasureGroup_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.MeasureGroups
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                        });

            var ctr = new ContainConstraint("Reseller Orders");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingMeasureGroup_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.MeasureGroups
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingMeasure_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Measures
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                                , new CaptionFilter(Target.MeasureGroups, "Reseller Orders")
                        });

            var ctr = new ContainConstraint("Reseller Order Count");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingMeasure_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Measures
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                                , new CaptionFilter(Target.MeasureGroups, "Reseller Orders")
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingMeasureWithoutMeasureGroup_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Measures
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                        });

            var ctr = new ContainConstraint("Reseller Order Count");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindNonExistingMeasureWithoutMeasureGroup_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Measures
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                        });

            var ctr = new ContainConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainConstraint_FindExistingMeasureWithCaseNonMatching_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Measures
                        , TargetType.Object
                        , new CaptionFilter[] { 
                                new CaptionFilter(Target.Perspectives, "Adventure Works") 
                                , new CaptionFilter(Target.MeasureGroups, "Reseller Orders")
                        });

            var ctr = new ContainConstraint("Reseller Order Count".ToLower());
            ctr = ctr.IgnoreCase;

            //Method under test
            Assert.That(discovery, ctr);

        }
    }

}
