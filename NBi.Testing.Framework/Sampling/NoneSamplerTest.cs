using Moq;
using NBi.Core.ResultSet;
using NBi.Framework.FailureMessage;
using NBi.Framework.FailureMessage.Json;
using NBi.Framework.Sampling;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Framework.Testing.Sampling
{
    public class NoneSamplerTest
    {
        [Test]
        public void GetResult_NonEmptyList_Empty()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new NoneSampler<int>();
            sampler.Build(values);

            Assert.That(sampler.GetResult(), Is.Empty);
        }

        [Test]
        public void IsSampled_NonEmptyList_True()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new NoneSampler<int>();
            sampler.Build(values);

            Assert.That(sampler.GetIsSampled, Is.True);
        }

        [Test]
        public void GetExcludedRowCount_NonEmptyList_10()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new NoneSampler<int>();
            sampler.Build(values);

            Assert.That(sampler.GetExcludedRowCount, Is.EqualTo(10));
        }

        [Test]
        public void GetResult_EmptyList_Empty()
        {
            var values = new int[0];

            var sampler = new NoneSampler<int>();
            sampler.Build(values);

            Assert.That(sampler.GetResult(), Is.Empty);
        }

        [Test]
        public void IsSampled_EmptyList_True()
        {
            var values = new int[0];

            var sampler = new NoneSampler<int>();
            sampler.Build(values);

            Assert.That(sampler.GetIsSampled, Is.True);
        }

        [Test]
        public void GetExcludedRowCount_EmptyList_0()
        {
            var values = new int[0];

            var sampler = new NoneSampler<int>();
            sampler.Build(values);

            Assert.That(sampler.GetExcludedRowCount, Is.EqualTo(0));
        }

    }
}
