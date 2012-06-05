#region Using directives
using NBi.Core.Analysis;
using NBi.Core.Analysis.Metadata;
using NBi.NUnit.Structure;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Acceptance.NUnit.Structure
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
        public void ContainsConstraint_FindExistingHierarchyBellowSpecificDimension_Success()
        {
            var mq = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            mq.Path = "[Date]";
            mq.Perspective = "Finances";
            
            var ctr = new ContainsConstraint("Calendar");

            //Method under test
            Assert.That(mq, ctr);

        }

        [Test, Category("Olap cube")]
        public void ContainsConstraint_FindNonExistingHierarchyBellowSpecificDimension_Failure()
        {
            var mq = new DiscoverCommand(ConnectionStringReader.GetAdomd());
            mq.Path = "[Date]";
            mq.Perspective = "Finances";

            var ctr = new ContainsConstraint("Not existing");

            //Method under test
            Assert.That(ctr.Matches(mq), Is.False);
        }

    }

}
