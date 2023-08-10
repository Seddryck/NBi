using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Scalar.Resolver
{
    public class EnvironmentScalarResolverTest
    {
        [OneTimeSetUp]
        public void CreateEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("NBiTestingProcess", "the_value_process", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("NBiTestingUser", "the_value_user", EnvironmentVariableTarget.User);
        }

        [OneTimeTearDown]
        public void DeleteEnvironmentVariable()
        {
            Environment.SetEnvironmentVariable("NBiTestingProcess", null, EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("NBiTestingUser", null, EnvironmentVariableTarget.User);
        }

        [Test]
        public void Execute_EnvVarUser_CorrectRead()
        {
            var args = new EnvironmentScalarResolverArgs("NBiTestingUser");
            var resolver = new EnvironmentScalarResolver<string>(args);

            var output = resolver.Execute();

            Assert.That(output, Is.EqualTo("the_value_user"));
        }

        [Test]
        public void Execute_EnvVarProcess_CorrectRead()
        {
            var args = new EnvironmentScalarResolverArgs("NBiTestingProcess");
            var resolver = new EnvironmentScalarResolver<string>(args);

            var output = resolver.Execute();

            Assert.That(output, Is.EqualTo("the_value_process"));
        }

        [Test]
        public void Execute_EnvVarMissing_CorrectRead()
        {
            var args = new EnvironmentScalarResolverArgs("NBiTestingMissing");
            var resolver = new EnvironmentScalarResolver<string>(args);

            var output = resolver.Execute();

            Assert.That(output, Is.Null);
        }
    }
}
