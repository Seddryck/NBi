#region Using directives
using System.Collections.Generic;
using NBi.Core.Analysis.Request;
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
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , null, null, null, null, null, null, null
                        );

            var ctr = new ContainsConstraint("Adventure Works");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingPerspective_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , null, null, null, null, null, null, null
                        );

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindAllExistingPerspectives_Success()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , null, null, null, null, null, null, null
                        );

            var expected = new string[] { "Adventure Works", "Channel Sales", "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var ctr = new ContainsConstraint(expected);
            ctr = ctr.Exactly;

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindAllExistingPerspectivesOneMissing_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , null, null, null, null, null, null, null
                        );

            var expectedStrings = new string[] { "Adventure Works", "Channel Sales", "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var expected = new List<string>();
            expected.AddRange(expectedStrings);
            expected.RemoveAt(0);
            var ctr = new ContainsConstraint(expected);
            ctr = ctr.Exactly;

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindAllExistingPerspectivesOneMore_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , null, null, null, null, null, null, null
                        );

            var expectedStrings = new string[] { "Adventure Works", "Channel Sales", "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var expected = new List<string>();
            expected.AddRange(expectedStrings);
            expected.Add("Unexpected");
            var ctr = new ContainsConstraint(expected);
            ctr = ctr.Exactly;

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindSubsetExistingPerspectives_Success()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , null, null, null, null, null, null, null
                        );

            var expected = new string[] { "Adventure Works", "Channel Sales", "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var ctr = new ContainsConstraint(expected);
            ctr = ctr.Exactly;

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindSubsetExistingPerspectivesOneMissing_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , null, null, null, null, null, null, null
                        );

            var expectedStrings = new string[] { "Adventure Works", "Channel Sales", "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var expected = new List<string>();
            expected.AddRange(expectedStrings);
            expected.RemoveAt(0);
            
            var ctr = new ContainsConstraint(expected);

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindSubsetExistingPerspectivesOneMore_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , null, null, null, null, null, null, null
                        );

            var expectedStrings = new string[] { "Adventure Works", "Channel Sales", "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var expected = new List<string>();
            expected.AddRange(expectedStrings);
            expected.Add("My new perspective");

            var ctr = new ContainsConstraint(expected);

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.True);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingDimension_Success()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , "Adventure Works"
                        , null, null, null, null, null, null
                        );

            var ctr = new ContainsConstraint("Product");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Dimensions
                        , "Adventure Works"
                        , null, null, null, null, null, null
                        );

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }


        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingHierarchyBellowSpecificDimension_Success()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Hierarchies
                        , "Adventure Works"
                        , null, null, null
                        , "Product"
                        , null, null
                        );
                        

            var ctr = new ContainsConstraint("Product Model Lines");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingHierarchyBellowSpecificDimension_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Hierarchies
                        , "Adventure Works"
                        , null, null, null
                        , "Product"
                        , null, null
                        );

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }


        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingLevel_Success()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Levels
                        , "Adventure Works"
                        , null, null, null
                        , "Customer"
                        , "Customer Geography"
                        , null
                        );

            var ctr = new ContainsConstraint("City");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingLevel_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Levels
                        , "Adventure Works"
                        , null, null, null
                        , "Customer"
                        , "Customer Geography"
                        , null
                        );

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingMeasureGroup_Success()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , "Adventure Works"
                        , null, null, null, null, null, null
                        );

            var ctr = new ContainsConstraint("Reseller Orders");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.MeasureGroups
                        , "Adventure Works"
                        , null, null, null, null, null, null
                        );

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingMeasure_Success()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Measures
                        , "Adventure Works"
                        , "Reseller Orders"
                        , null, null, null, null, null
                        );

            var ctr = new ContainsConstraint("Reseller Order Count");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingMeasure_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                         ConnectionStringReader.GetAdomd()
                         , DiscoveryTarget.Measures
                         , "Adventure Works"
                         , "Reseller Orders"
                         , null, null, null, null, null
                         );

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindExistingMeasureWithoutMeasureGroup_Success()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Measures
                        , "Adventure Works"
                        , null
                        , null, null, null, null, null
                        );

            var ctr = new ContainsConstraint("Reseller Order Count");

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingMeasureWithoutMeasureGroup_Failure()
        {
            var discovery = new DiscoveryRequestFactory().Build(
                         ConnectionStringReader.GetAdomd()
                         , DiscoveryTarget.Measures
                         , "Adventure Works"
                         , null
                         , null, null, null, null, null
                         );

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);
        }

        

    }

}
