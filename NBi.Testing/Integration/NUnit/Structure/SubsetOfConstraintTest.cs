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
    public class SubsetOfConstraintTest
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
        public void Matches_ActualEqualToExpectation_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Perspectives
                        , TargetType.Object
                        , new CaptionFilter[] { });

            var expected = new string[] { "Adventure Works", "Channel Sales", "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var ctr = new ContainedInConstraint(expected);

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void Matches_ActualEqualToExpectationCaseNonMatching_Success()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Perspectives
                        , TargetType.Object
                        , new CaptionFilter[] { });

            var expected = new string[] { "Adventure Works".ToUpper(), "Channel Sales".ToLower(), "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var ctr = new ContainedInConstraint(expected);
            ctr = ctr.IgnoreCase;

            //Method under test
            Assert.That(discovery, ctr);

        }

        [Test, Category("Olap cube")]
        public void Matches_ActualMoreThanExpectation_Failure()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Perspectives
                        , TargetType.Object
                        , new CaptionFilter[] { });

            var expectedStrings = new string[] { "Adventure Works", "Channel Sales", "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var expected = new List<string>();
            expected.AddRange(expectedStrings);
            expected.RemoveAt(0);
            var ctr = new ContainedInConstraint(expected);

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.False);

        }

        [Test, Category("Olap cube")]
        public void Matches_ActualSubsetOfExpectation_Sucess()
        {
            var provider = new StructureDiscoveryFactoryProvider();
            var factory = provider.Instantiate(ConnectionStringReader.GetAdomd());
            var discovery = factory.Instantiate(
                        Target.Perspectives
                        , TargetType.Object
                        , new CaptionFilter[] { });

            var expectedStrings = new string[] { "Adventure Works", "Channel Sales", "Direct Sales", "Finance", "Mined Customers", "Sales Summary", "Sales Targets" };
            var expected = new List<string>();
            expected.AddRange(expectedStrings);
            expected.Add("New perspective");
            var ctr = new ContainedInConstraint(expected);

            //Method under test
            Assert.That(ctr.Matches(discovery), Is.True);

        }
    }
}