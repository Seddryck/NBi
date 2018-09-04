using NUnit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Text;
using System.Threading.Tasks;
using Moq;
using NBi.NUnit.Runtime.Embed.Result;

namespace NBi.Testing.Unit.NUnit.Runtime.Embed.Result
{
    public class FlatResultBuilderTest
    {
        [Test]
        public void Execute_TestResult_ReturnAggregation()
        {
            var test = Mock.Of<ITest>(
                x => x.IsSuite == false 
                && x.TestName==new TestName() { FullName = "TestSuite.Test" }
                && x.Properties == new Dictionary<string, object>() { { "Identifier", "122" } });

            var raw = new TestResult(test);
            raw.SetResult(ResultState.Error, "My message", string.Empty);
            var builder = new FlatResultBuilder();
            var agg = builder.Execute(raw);
            Assert.That(agg.Count, Is.EqualTo(1));
        }

        [Test]
        public void Execute_OneSuccess_CountOK()
        {
            var test = Mock.Of<ITest>(
                x => x.IsSuite == false
                && x.TestName == new TestName() { FullName = "TestSuite.Test" }
                && x.Properties == new Dictionary<string, object>() { { "Identifier", "122" } });

            var raw = new TestResult(test);
            raw.SetResult(ResultState.Success, "My message", string.Empty);
            var builder = new FlatResultBuilder();
            var agg = builder.Execute(raw);
            Assert.That(agg.Successes, Is.EqualTo(1));
            Assert.That(agg.Failures, Is.EqualTo(0));
        }

        [Test]
        public void Execute_OneFailure_CountOK()
        {
            var test = Mock.Of<ITest>(
                x => x.IsSuite == false
                && x.TestName == new TestName() { FullName = "TestSuite.Test" }
                && x.Properties == new Dictionary<string, object>() { { "Identifier", "122" } });

            var raw = new TestResult(test);
            raw.SetResult(ResultState.Failure, "My message", string.Empty);
            var builder = new FlatResultBuilder();
            var agg = builder.Execute(raw);
            Assert.That(agg.Successes, Is.EqualTo(0));
            Assert.That(agg.Failures, Is.EqualTo(1));
        }


    }
}
