#region Using directives
using NBi.Core.Analysis.Metadata;
using NBi.NUnit;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Acceptance.NUnit
{
    [TestFixture]
    public class ExistConstraintTest
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
        public void CubeStructureRealImplementation_ExistConstraint_Success()
        {
            var mq = new MetadataQuery();
            mq.Path = "[Date]";
            mq.Perspective = "Finances";
            mq.ConnectionString = ConnectionStringReader.GetAdomd();

            var ctr = new NBi.NUnit.Element.ContainsConstraint();
            ctr = ctr.Caption("Calendar");

            //Method under test
            Assert.That(mq, ctr);

        }

    }

}
