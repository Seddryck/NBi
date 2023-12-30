using Expressif.Predicates.Text;
using NBi.Core.Calculation.Grouping.CaseBased;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using NBi.Core.Calculation.Asserting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.Calculation.Grouping.ColumnBased
{
    public class CaseGroupingTest
    {
        [Test]
        public void Execute_SingleColumn_TwoGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { ["alpha", 1], ["beta", 2], ["BETA", 3], new object[] { "alpha", 4 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            var lowerCase = new Predication(new Predicate(new LowerCase()), new ColumnOrdinalIdentifier(0));
            var upperCase = new Predication(new Predicate(new UpperCase()), new ColumnOrdinalIdentifier(0));

            var grouping = new CaseGrouping(new IPredication[] { lowerCase, upperCase }, new Context());

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.ElementAt(0).Value.Rows.Count, Is.EqualTo(3));
            Assert.That(result.ElementAt(1).Value.Rows.Count, Is.EqualTo(1));
        }

        [Test]
        [Ignore("Can't have two inputs for a predication with current version of Expressif")]
        public void Execute_TwoColumns_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { ["alpha", "1", 10], ["ALPHA", "1", 20], ["beta", "2", 30], new object[] { "ALPHA", "2", 40 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            
            var lowerCase = new Predication(new Predicate(new LowerCase()), new ColumnOrdinalIdentifier(0));
            //var upperCase = new Predication(
            //                    new Expressif.Predicates.PredicateCombiner()
            //                        .With(new UpperCase(), new ColumnOrdinalIdentifier(0))
            //                        .And(new EquivalentTo(() => new LiteralScalarResolver<string>("1").Execute()!), new ColumnOrdinalIdentifier(1))
            //                        .Build()
            //                );
            var upperCase = new Predication(new Predicate(new UpperCase()), new ColumnOrdinalIdentifier(0));

            var grouping = new CaseGrouping([ lowerCase, upperCase ], Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result.ElementAt(0).Value.Rows.Count, Is.EqualTo(2));
            Assert.That(result.ElementAt(1).Value.Rows.Count, Is.EqualTo(1));
            Assert.That(result.ElementAt(2).Value.Rows.Count, Is.EqualTo(1));
        }

        [Test]
        [Ignore("Can't have two inputs for a predication with current version of Expressif")]
        public void Execute_TwoColumnsWithContext_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { ["alpha", "1", "1"], ["ALPHA", "1", "1"], ["beta", "2", "2"], new object[] { "ALPHA", "2", "4" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var lowerCase = new Predication(new Predicate(new LowerCase()), new ColumnOrdinalIdentifier(0));
            //var upperCase = new Predication(
            //                    new Expressif.Predicates.PredicateCombiner()
            //                        .With(new UpperCase(), new ColumnOrdinalIdentifier(0))
            //                        .And(new EquivalentTo(() => new ContextScalarResolverArgs(Context.None, new ColumnOrdinalIdentifier(2)).Execute()!), new ColumnOrdinalIdentifier(1))
            //                        .Build()
            //                );
            var upperCase = new Predication(new Predicate(new UpperCase()), new ColumnOrdinalIdentifier(0));

            var grouping = new CaseGrouping(new IPredication[] { lowerCase, upperCase }, Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result.ElementAt(0).Value.Rows.Count, Is.EqualTo(2));
            Assert.That(result.ElementAt(1).Value.Rows.Count, Is.EqualTo(1));
            Assert.That(result.ElementAt(2).Value.Rows.Count, Is.EqualTo(1));
        }
    }
}
