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
    public class BasicSamplerTest
    {
        [Test]
        public void GetResult_NonEmptyList_Empty()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(20,20);
            sampler.Build(values);

            Assert.That(sampler.GetResult().Count(), Is.EqualTo(10));
        }

        [Test]
        public void IsSampled_NonEmptyList_False()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(20, 20);
            sampler.Build(values);

            Assert.That(sampler.GetIsSampled, Is.False);
        }

        [Test]
        public void GetExcludedRowCount_NonEmptyList_0()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(20, 20);
            sampler.Build(values);

            Assert.That(sampler.GetExcludedRowCount, Is.EqualTo(0));
        }

        [Test]
        public void GetResult_EmptyList_Empty()
        {
            var values = new int[0];

            var sampler = new BasicSampler<int>(20, 20);
            sampler.Build(values);

            Assert.That(sampler.GetResult(), Is.Empty);
        }

        [Test]
        public void IsSampled_EmptyList_False()
        {
            var values = new int[0];

            var sampler = new BasicSampler<int>(20, 20);
            sampler.Build(values);

            Assert.That(sampler.GetIsSampled, Is.False);
        }

        [Test]
        public void GetExcludedRowCount_EmptyList_0()
        {
            var values = new int[0];

            var sampler = new BasicSampler<int>(20, 20);
            sampler.Build(values);

            Assert.That(sampler.GetExcludedRowCount, Is.EqualTo(0));
        }

        [Test]
        public void GetResult_LargerList_3Members()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(3, 3);
            sampler.Build(values);

            Assert.That(sampler.GetResult().Count(), Is.EqualTo(3));
        }

        [Test]
        public void IsSampled_LargerList_True()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(3, 3);
            sampler.Build(values);

            Assert.That(sampler.GetIsSampled, Is.True);
        }

        [Test]
        public void GetExcludedRowCount_LargerList_7()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(3, 3);
            sampler.Build(values);

            Assert.That(sampler.GetExcludedRowCount, Is.EqualTo(7));
        }

        [Test]
        public void GetResult_MaxSampled_3Members()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(3, 4);
            sampler.Build(values);

            Assert.That(sampler.GetResult().Count(), Is.EqualTo(4));
        }

        [Test]
        public void IsSampled_MaxSampled_True()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(3, 4);
            sampler.Build(values);

            Assert.That(sampler.GetIsSampled, Is.True);
        }

        [Test]
        public void GetExcludedRowCount_MaxSampled_7()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(3, 4);
            sampler.Build(values);

            Assert.That(sampler.GetExcludedRowCount, Is.EqualTo(6));
        }

        [Test]
        public void GetResult_Threshold_3Members()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(15, 4);
            sampler.Build(values);

            Assert.That(sampler.GetResult().Count(), Is.EqualTo(10));
        }

        [Test]
        public void IsSampled_Threshold_False()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(15, 4);
            sampler.Build(values);

            Assert.That(sampler.GetIsSampled, Is.False);
        }

        [Test]
        public void GetExcludedRowCount_Threshold_0()
        {
            var values = new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };

            var sampler = new BasicSampler<int>(15, 4);
            sampler.Build(values);

            Assert.That(sampler.GetExcludedRowCount, Is.EqualTo(0));
        }

    }
}
