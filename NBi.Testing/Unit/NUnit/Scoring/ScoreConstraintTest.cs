using System.Data.SqlClient;
using Moq;
using NBi.Core.Query;
using NBi.NUnit.Query;
using NUnit.Framework;
using NBi.Core.Query.Performance;
using System;
using NBi.Extensibility.Query;
using NBi.NUnit.Scoring;
using NBi.Core.Scalar.Resolver;

namespace NBi.Testing.Unit.NUnit.Query
{
    [TestFixture]
    public class ScoreConstraintTest
    {

        #region Setup & Teardown

        [SetUp]
        public void SetUp()
        {
           
        }

        [TearDown]
        public void TearDown()
        {
        }

        #endregion

        [Test]
        public void Matches_LessThan_ReturnFalse()
        {
            var ctr = new ScoreConstraint(0.75m);
            var res = ctr.Matches(0.62m);
            Assert.That(res, Is.False);
        }

        [Test]
        public void Matches_MoreThan_ReturnTrue()
        {
            var ctr = new ScoreConstraint(0.75m);
            var res = ctr.Matches(0.89m);
            Assert.That(res, Is.True);
        }

        [Test]
        public void Matches_Equal_ReturnTrue()
        {
            var ctr = new ScoreConstraint(0.75m);
            var res = ctr.Matches(0.75m);
            Assert.That(res, Is.True);
        }


        [Test]
        public void Matches_ScalarResolver_IsExecuted()
        {
            var resolverMock = new Mock<IScalarResolver<decimal>>();
            resolverMock.Setup(x => x.Execute()).Returns(0.62m);

            var ctr = new ScoreConstraint(0.75m);
            var res = ctr.Matches(resolverMock.Object);
            Mock.VerifyAll(resolverMock);
        }
    }
}
