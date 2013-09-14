using System;
using System.Linq;
using NBi.Core.ResultSet.Comparer;
using NUnit.Framework;

namespace NBi.Testing.Unit.Core.ResultSet.Comparer
{
    [TestFixture]
    public class NumericRoundingTest
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
        [TestCase(105, 20, Rounding.RoudingStyle.Floor, 100)]
        [TestCase(105, 2, Rounding.RoudingStyle.Floor, 104)]
        [TestCase(105, 2, Rounding.RoudingStyle.Round, 104)]
        [TestCase(108, 10, Rounding.RoudingStyle.Round, 110)]
        [TestCase(105, 20, Rounding.RoudingStyle.Ceiling, 120)]
        [TestCase(105, 2, Rounding.RoudingStyle.Ceiling, 106)]
        [TestCase(105, 5, Rounding.RoudingStyle.Floor, 105)]
        [TestCase(105, 5, Rounding.RoudingStyle.Ceiling, 105)]
        [TestCase(105, 5, Rounding.RoudingStyle.Round, 105)]
        public void GetValue_ValueStepStyle_NewValue(decimal value, int step, Rounding.RoudingStyle roundingStyle, decimal newValue)
        {
            var rounder = new NumericRounding(step, roundingStyle);
            
            Assert.That(rounder.GetValue(value), Is.EqualTo(newValue));
        }
    }
}
