using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Neo4j.Acceptance
{
    public class Neo4jRuntimeOverrider : NBi.Testing.Acceptance.RuntimeOverrider
    {

        [Test]
        [TestCase("ResultSetEqualToResultSet.nbits")]
        public override void RunPositiveTestSuiteWithConfig(string filename)
        {
            base.RunPositiveTestSuiteWithConfig(filename);
        }
    }
}
