#region Using directives
using System.Collections;
using System.Data;
using System.Data.OleDb;
using Moq;
using NBi.Core;
using NBi.Core.Analysis.Query;
using NBi.Core.Database;
using NUnit.Framework;
#endregion

namespace NBi.Testing.Unit.Core.Database
{
    [TestFixture]
    public class CollectionEngineTest
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

        [Test]
        public void ValidateExactlySpecified_ExpectedAndActualAreEqual_ReturnResultSuccess()
        {
            var ce = new CollectionEngine();
            ce.Exactly = 2;

            var coll = new ArrayList();
            for (int i = 0; i < 2; i++)
                coll.Add(new object());

            Assert.That(ce.Validate(coll).ToBoolean(), Is.True); 
        }

        [Test]
        public void ValidateExactlySpecified_ExpectedAndActualAreDifferent_ReturnResultFailed()
        {
            var ce = new CollectionEngine();
            ce.Exactly = 3;

            var coll = new ArrayList();
            for (int i = 0; i < 2; i++)
                coll.Add(new object());
            
            Assert.That(ce.Validate(coll).ToBoolean(), Is.False);
        }

        [Test]
        public void ValidateMoreThanSpecified_ExpectedAndActualAreMatching_ReturnResultSuccess()
        {
            var ce = new CollectionEngine();
            ce.MoreThan = 1;

            var coll = new ArrayList();
            for (int i = 0; i < 2; i++)
                coll.Add(new object());

            Assert.That(ce.Validate(coll).ToBoolean(), Is.True);
        }

        [Test]
        public void ValidateMoreThanSpecified_ExpectedAndActualAreNotMatching_ReturnResultFailed()
        {
            var ce = new CollectionEngine();
            ce.MoreThan = 2;

            var coll = new ArrayList();
            for (int i = 0; i < 2; i++)
                coll.Add(new object());

            Assert.That(ce.Validate(coll).ToBoolean(), Is.False);
        }

        [Test]
        public void ValidateLessThanSpecified_ExpectedAndActualAreMatching_ReturnResultSuccess()
        {
            var ce = new CollectionEngine();
            ce.LessThan = 3;

            var coll = new ArrayList();
            for (int i = 0; i < 2; i++)
                coll.Add(new object());

            Assert.That(ce.Validate(coll).ToBoolean(), Is.True);
        }

        [Test]
        public void ValidateLessThanSpecified_ExpectedAndActualAreNotMatching_ReturnResultFailed()
        {
            var ce = new CollectionEngine();
            ce.LessThan = 2;

            var coll = new ArrayList();
            for (int i = 0; i < 2; i++)
                coll.Add(new object());

            Assert.That(ce.Validate(coll).ToBoolean(), Is.False);
        }

        [Test]
        public void ValidateLessThanAndMoreThanSpecified_ExpectedAndActualAreMatching_ReturnResultTrue()
        {
            var ce = new CollectionEngine();
            ce.MoreThan = 1;
            ce.LessThan = 3;

            var coll = new ArrayList();
            for (int i = 0; i < 2; i++)
                coll.Add(new object());

            Assert.That(ce.Validate(coll).ToBoolean(), Is.True);
        }

        [Test]
        public void ValidateLessThanAndMoreThanSpecified_SecondExpectedAndActualAreNotMatching_ReturnResultFalse()
        {
            var ce = new CollectionEngine();
            ce.MoreThan = 2;
            ce.LessThan = 4;

            var coll = new ArrayList();
            for (int i = 0; i < 2; i++)
                coll.Add(new object());

            Assert.That(ce.Validate(coll).ToBoolean(), Is.False);
        }

        [Test]
        public void ValidateLessThanAndMoreThanSpecified_FirstExpectedAndActualAreNotMatching_ReturnResultFalse()
        {
            var ce = new CollectionEngine();
            ce.MoreThan = 0;
            ce.LessThan = 2;

            var coll = new ArrayList();
            for (int i = 0; i < 2; i++)
                coll.Add(new object());

            Assert.That(ce.Validate(coll).ToBoolean(), Is.False);
        }

    }
}
