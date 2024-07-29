using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Sequence.Resolver;
using NBi.Core.Variable;
using NBi.Extensibility;
using NBi.Extensibility.Resolving;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Resolver
{
    public class IterativeResultSetResolverTest
    {
        private class DummyResultSetResolver : IResultSetResolver
        {
            private IDictionary<string, IVariable> Variables { get; }

            private string VariableName { get; }

            public DummyResultSetResolver(IDictionary<string, IVariable> variables, string variableName)
                => (Variables, VariableName) = (variables, variableName);

            public IResultSet Execute()
            {
                var args = new ObjectsResultSetResolverArgs(new[] { new object?[] { Variables[VariableName].GetValue() } });
                var resolver = new ObjectsResultSetResolver(args);
                return resolver.Execute();
            }
        }

        [Test]
        public void Execute_TwoElements_TwoRows()
        {
            var variables = new Dictionary<string, IVariable>();

            var sequenceResolver = new ListSequenceResolver<decimal>(
                new ListSequenceResolverArgs(
                    new List<IScalarResolver>() { new LiteralScalarResolver<decimal>(0), new LiteralScalarResolver<decimal>(1) }
                ));

            var resolver = new IterativeResultSetResolver(sequenceResolver, "i", variables, new DummyResultSetResolver(variables, "i"));
            var result = resolver.Execute();

            Assert.That(result.Rows.Count, Is.EqualTo(2));
            Assert.That(result[0][0], Is.EqualTo(0));
            Assert.That(result[1][0], Is.EqualTo(1));
        }

        [Test]
        public void Execute_ThreeElements_ThreeRows()
        {
            var variables = new Dictionary<string, IVariable>();

            var sequenceResolver = new ListSequenceResolver<decimal>(
                new ListSequenceResolverArgs(
                    new List<IScalarResolver>() {
                        new LiteralScalarResolver<decimal>(0)
                        , new LiteralScalarResolver<decimal>(1)
                        , new LiteralScalarResolver<decimal>(2)
                    }
                ));

            var resolver = new IterativeResultSetResolver(sequenceResolver, "i", variables, new DummyResultSetResolver(variables, "i"));
            var result = resolver.Execute();

            Assert.That(result.Rows.Count, Is.EqualTo(3));
            Assert.That(result[0][0], Is.EqualTo(0));
            Assert.That(result[1][0], Is.EqualTo(1));
            Assert.That(result[2][0], Is.EqualTo(2));
        }

        [Test]
        public void Execute_OneElement_OneRow()
        {
            var variables = new Dictionary<string, IVariable>();

            var sequenceResolver = new ListSequenceResolver<decimal>(
                new ListSequenceResolverArgs(
                    new List<IScalarResolver>() {
                        new LiteralScalarResolver<decimal>(0)
                    }
                ));

            var resolver = new IterativeResultSetResolver(sequenceResolver, "i", variables, new DummyResultSetResolver(variables, "i"));
            var result = resolver.Execute();

            Assert.That(result.Rows.Count, Is.EqualTo(1));
            Assert.That(result[0][0], Is.EqualTo(0));
        }

        [Test]
        public void Execute_ZeroElements_Empty()
        {
            var variables = new Dictionary<string, IVariable>();

            var sequenceResolver = new ListSequenceResolver<decimal>(
                new ListSequenceResolverArgs(
                    new List<IScalarResolver>() {}
                ));

            var resolver = new IterativeResultSetResolver(sequenceResolver, "i", variables, new DummyResultSetResolver(variables, "i"));
            var result = resolver.Execute();

            Assert.That(result.Rows.Count, Is.EqualTo(0));
            Assert.That(result.ColumnCount, Is.EqualTo(0));
        }
    }
}
