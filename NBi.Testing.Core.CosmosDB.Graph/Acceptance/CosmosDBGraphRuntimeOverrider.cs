using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.CosmosDb.Graph.Acceptance
{
    public class CosmosDbGraphRuntimeOverrider : NBi.Testing.Acceptance.RuntimeOverrider
    {

        [Test]
        [TestCase("ResultSetEqualToResultSet.nbits")]
        public override void RunPositiveTestSuiteWithConfig(string filename)
        {
            base.RunPositiveTestSuiteWithConfig(filename);
        }
    }
}
