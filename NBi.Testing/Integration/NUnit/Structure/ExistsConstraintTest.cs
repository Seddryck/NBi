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
    public class ExistsConstraintTest
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
        public void ExistsConstraint_ExistingPerspectiveButWrongCaseWithIgnoreCaseFalse_Failure()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works".ToLower(), DiscoveryTarget.Perspectives)
                        });

            var ctr = new ExistsConstraint();

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(discovery, ctr); });
        }

        [Test, Category("Olap cube")]
        public void ExistsConstraint_ExistingPerspectiveButWrongCaseWithIgnoreCaseTrue_Success()
        {
            var discovery = new DiscoveryRequestFactory().BuildDirect(
                        ConnectionStringReader.GetAdomd()
                        , DiscoveryTarget.Perspectives
                        , new List<IFilter>() { 
                            new CaptionFilter("Adventure Works", DiscoveryTarget.Perspectives),
                            new CaptionFilter("Date", DiscoveryTarget.Dimensions)
                        });

            var ctr = new ExistsConstraint();
            ctr = ctr.IgnoreCase;

            //Method under test
            Assert.That(discovery, ctr);
        }

    }

}
