using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.CosmosDb.Acceptance
{
    public class CosmosDbRuntimeOverrider : NBi.Testing.Acceptance.RuntimeOverrider
    {
        [SetUp]
        public void Setup()
        {
            var tsSetup = new TestSuiteSetup();
            tsSetup.Init();

            CopyConnectionStringUserConfig();
        }

        public void CopyConnectionStringUserConfig()
        {
            var fileName = "ConnectionString.user.config";
            if (System.IO.File.Exists(fileName))
                System.IO.File.Copy(fileName, $@"Acceptance\Resources\Positive\{fileName}", true);
        }
        

        [Test]
        [TestCase("ResultSetEqualToResultSet.nbits")]
        public override void RunPositiveTestSuiteWithConfig(string filename) => base.RunPositiveTestSuiteWithConfig(filename);

        public override void RunIgnoredTests(string filename) => throw new NotImplementedException();

        public override void RunNegativeTestSuite(string filename) => throw new NotImplementedException();

        public override void RunNegativeTestSuiteWithConfig(string filename) => throw new NotImplementedException();

        public override void RunPositiveTestSuite(string filename) => throw new NotImplementedException();
    }
}
