using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.Core.Injection;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NBi.NUnit.Builder.Helper;
using NBi.Xml;

namespace NBi.NUnit.Runtime
{
    public class TestInstanceEngine
    {
        private readonly ServiceLocator locator;
        public TestInstanceEngine(ServiceLocator locator)
        {
            this.locator = locator;
        }

        public IEnumerable<TestInstance> Develop(InstanceXml instance)
        {
            var factory = locator.GetScalarResolverFactory();
            var builder = new ScalarResolverArgsBuilder(locator);

            foreach (var value in instance.Variation.Values)
            {
                var inst = new TestInstance();
                builder.Setup(value);
                builder.Build();
                var resolver = factory.Instantiate<object>(builder.GetArgs());
                var variable = new TestVariable(resolver);
                inst.Variables.Add(instance.Variation.Name, variable);
                yield return inst;
            }
        }
    }
}
