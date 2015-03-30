#region Using directives
using System.Collections.Generic;
using NBi.NUnit.Structure;
using NUnit.Framework;
using NBi.Core.Structure.Olap;
using Microsoft.AnalysisServices.AdomdClient;
using NBi.Core.Structure;
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
            var connection = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(connection);
            var command = factory.Instantiate(
                            Target.Perspectives
                            , TargetType.Object
                            , new CaptionFilter[] { 
                        });

            var ctr = new ExistsConstraint("Adventure Works".ToLower());

            //Method under test
            Assert.Throws<AssertionException>(delegate { Assert.That(command, ctr); });
        }

        [Test, Category("Olap cube")]
        public void ExistsConstraint_ExistingPerspectiveButWrongCaseWithIgnoreCaseTrue_Success()
        {
            var connection = new AdomdConnection(ConnectionStringReader.GetAdomd());
            var factory = new OlapStructureDiscoveryFactory(connection);
            var command = factory.Instantiate(
                            Target.Perspectives
                            , TargetType.Object
                            , new CaptionFilter[] { 
                        });

            var ctr = new ExistsConstraint("Adventure Works".ToLower());
            ctr = ctr.IgnoreCase;

            //Method under test
            Assert.That(command, ctr);
        }

    }

}
