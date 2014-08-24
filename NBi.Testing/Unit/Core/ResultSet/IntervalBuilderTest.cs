using System;
using System.Linq;
using NBi.Core.ResultSet;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet
{
    [TestFixture]
    public class IntervalBuilderTest
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
        public void Build_ClosedClosed_ReturnBothClosed()
        {
            var builder = new IntervalBuilder(" [10 ; 200 ] ");
            builder.Build();
            var interval = builder.GetInterval();

            Assert.That(interval.Left.IsClosed, Is.True);
            Assert.That(interval.Right.IsClosed, Is.True);
        }

        [Test]
        public void Build_OpenOpen_ReturnBothNotClosed()
        {
            var builder = new IntervalBuilder(" ] 10 ; 200[");
            builder.Build();
            var interval = builder.GetInterval();

            Assert.That(interval.Left.IsClosed, Is.False);
            Assert.That(interval.Right.IsClosed, Is.False);
        }

        [Test]
        public void Build_OpenClosed_ReturnOpenClosed()
        {
            var builder = new IntervalBuilder(" ] 10 ; 200]");
            builder.Build();
            var interval = builder.GetInterval();

            Assert.That(interval.Left.IsOpen, Is.True);
            Assert.That(interval.Right.IsClosed, Is.True);
        }

        [Test]
        public void ToString_OpenClosed_ReturnCorrectDisplay()
        {
            var builder = new IntervalBuilder(" ] 10 ; 200]");
            builder.Build();
            var interval = builder.GetInterval();

            Assert.That(interval.ToString(), Is.EqualTo("]10;200]"));
        }

        [Test]
        public void ToString_ClosedOpen_ReturnCorrectDisplay()
        {
            var builder = new IntervalBuilder(" [10 ; 200 [");
            builder.Build();
            var interval = builder.GetInterval();

            Assert.That(interval.ToString(), Is.EqualTo("[10;200["));
        }
    }
}
