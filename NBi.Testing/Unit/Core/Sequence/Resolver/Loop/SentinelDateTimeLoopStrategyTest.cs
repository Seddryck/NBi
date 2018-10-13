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
    public class SentinelDateTimeLoopStrategyTest
    {
        [Test]
        [TestCase(1, 5)]
        [TestCase(2, 5)]
        [TestCase(3, 4)]
        public void Run_parameters_CorrectResult(int stepDay, int expected)
        {
            var strategy = new SentinelDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2018, 1, 5), new TimeSpan(stepDay, 0, 0, 0));
            var final = new DateTime(2018, 1, 1);
            while (strategy.IsOngoing())
                final = strategy.GetNext();
            Assert.That(final, Is.EqualTo(new DateTime(2018, 1, expected)));
        }

        [Test]
        public void GetNext_FirstTime_Seed()
        {
            var strategy = new SentinelDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2018, 1, 2), new TimeSpan(1, 0, 0, 0));
            Assert.That(strategy.GetNext(), Is.EqualTo(new DateTime(2018, 1, 1)));
        }

        [Test]
        public void IsOngoing_ZeroTimes_False()
        {
            var strategy = new SentinelDateTimeLoopStrategy(new DateTime(2018, 1, 3), new DateTime(2018, 1, 2), new TimeSpan(1, 0, 0, 0));
            Assert.That(strategy.IsOngoing(), Is.False);
        }

        [Test]
        public void IsOngoing_OneTimes_TrueThenFalse()
        {
            var strategy = new SentinelDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2018, 1, 1), new TimeSpan(1, 0, 0, 0));
            Assert.That(strategy.IsOngoing(), Is.True);
            strategy.GetNext();
            Assert.That(strategy.IsOngoing(), Is.False);
        }

        [Test]
        public void IsOngoing_NTimes_True()
        {
            var strategy = new SentinelDateTimeLoopStrategy(new DateTime(2018, 1, 1), new DateTime(2018, 1, 10), new TimeSpan(1, 0, 0, 0));
            Assert.That(strategy.IsOngoing(), Is.True);
        }
    }
}
