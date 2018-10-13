using NBi.Core.Sequence.Resolver.Loop;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.Sequence.Resolver.Loop
{
    [TestFixture]
    public class CountDateTimeLoopStrategyTest
    {
        [Test]
        [TestCase(5, 1, 5)]
        [TestCase(5, 2, 9)]
        [TestCase(1, 2, 1)]
        [TestCase(10, 0, 1)]
        public void Run_parameters_CorrectResult(int count, int stepDay, int expected)
        {
            var strategy = new CountDateTimeLoopStrategy(count, new DateTime(2018, 1, 1), new TimeSpan(stepDay, 0, 0, 0));
            var final = new DateTime(2018, 1, 1);
            while (strategy.IsOngoing())
                final = strategy.GetNext();
            Assert.That(final, Is.EqualTo(new DateTime(2018, 1, expected)));
        }

        [Test]
        public void GetNext_FirstTime_Seed()
        {
            var strategy = new CountDateTimeLoopStrategy(10, new DateTime(2018, 1, 1), new TimeSpan(1, 0, 0, 0));
            Assert.That(strategy.GetNext(), Is.EqualTo(new DateTime(2018, 1, 1)));
        }

        [Test]
        public void IsOngoing_ZeroTimes_False()
        {
            var strategy = new CountDateTimeLoopStrategy(0, new DateTime(2015, 1, 1), new TimeSpan(1, 0, 0, 0));
            Assert.That(strategy.IsOngoing(), Is.False);
        }

        [Test]
        public void IsOngoing_OneTimes_TrueThenFalse()
        {
            var strategy = new CountDateTimeLoopStrategy(1, new DateTime(2015, 1, 1), new TimeSpan(1, 0, 0, 0));
            Assert.That(strategy.IsOngoing(), Is.True);
            strategy.GetNext();
            Assert.That(strategy.IsOngoing(), Is.False);
        }

        [Test]
        public void IsOngoing_NTimes_True()
        {
            var strategy = new CountDateTimeLoopStrategy(10, new DateTime(2015, 1, 1), new TimeSpan(1, 0, 0, 0));
            Assert.That(strategy.IsOngoing(), Is.True);
        }
    }
}
