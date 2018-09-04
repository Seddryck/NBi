using NBi.NUnit.Runtime.Embed;
using NBi.NUnit.Runtime.Embed.Filter;
using NBi.NUnit.Runtime.Embed.Result;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Integration.NUnit.Runtime.Embed
{
    public class EngineTest
    {
        private const string ConfigFilePath = @"Integration\NUnit\Runtime\Embed\Resources\SmallTestSuite.config";

        [Test]
        public void Execute_FromConfigFile_Ran()
        {
            var embed = new Engine();
            var result = embed.Execute(ConfigFilePath);
            var builder = new FlatResultBuilder();
            var agg = builder.Execute(result);
            Assert.That(agg.Count, Is.EqualTo(2));
            Assert.That(agg.Successes, Is.EqualTo(1));
        }

        [Test]
        public void Execute_WithNameFilter_Ran()
        {
            var embed = new Engine();
            var filter = new NBiNameFilter("All-rows is doing the job for numeric info");
            var result = embed.Execute(ConfigFilePath, filter);
            var builder = new FlatResultBuilder();
            var agg = builder.Execute(result);
            Assert.That(agg.Count, Is.EqualTo(1));
        }

        [Test]
        public void Execute_WithPropertyFilter_Ran()
        {
            var embed = new Engine();
            var filter = new PropertyFilter("propertyName", "propertyValue");
            var result = embed.Execute(ConfigFilePath, filter);
            var builder = new FlatResultBuilder();
            var agg = builder.Execute(result);
            Assert.That(agg.Count, Is.EqualTo(1));
        }
    }
}
