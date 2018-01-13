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
        public override void RunPositiveTestSuiteWithConfig(string filename) => base.RunPositiveTestSuiteWithConfig(filename);

        public override void RunIgnoredTests(string filename) => throw new NotImplementedException();

        public override void RunNegativeTestSuite(string filename) => throw new NotImplementedException();

        public override void RunNegativeTestSuiteWithConfig(string filename) => throw new NotImplementedException();

        public override void RunPositiveTestSuite(string filename) => throw new NotImplementedException();
    }
}
