using NBi.Core;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Sequence.Resolver.Resources
{
    public class MyCustomClassWithParams : ISequenceResolver
    {
        private int Foo { get; }
        private DateTime Bar { get; }

        public MyCustomClassWithParams(DateTime bar, int foo)
            => (Bar, Foo) = (bar, foo);

        public IList Execute() => new DateTime[] { Bar.AddDays(-Foo), Bar.AddDays(Foo) }.ToList();

        object IResolver.Execute() => Execute();
    }
}
