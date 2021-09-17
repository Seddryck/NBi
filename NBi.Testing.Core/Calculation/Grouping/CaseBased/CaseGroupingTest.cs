using NBi.Core.Calculation.Grouping.CaseBased;
using NBi.Core.Calculation.Predicate.Text;
using NBi.Core.Calculation.Predication;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.Calculation.Grouping.ColumnBased
{
    public class CaseGroupingTest
    {
        [Test]
        public void Execute_SingleColumn_TwoGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", 1 }, new object[] { "beta", 2 }, new object[] { "BETA", 3 }, new object[] { "alpha", 4 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            var lowerCase = new SinglePredication(new TextLowerCase(false), new ColumnOrdinalIdentifier(0));
            var upperCase = new SinglePredication(new TextUpperCase(false), new ColumnOrdinalIdentifier(0));

            var grouping = new CaseGrouping(new IPredication[] { lowerCase, upperCase }, Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(2));
            Assert.That(result.ElementAt(0).Value.Rows.Count, Is.EqualTo(3));
            Assert.That(result.ElementAt(1).Value.Rows.Count, Is.EqualTo(1));
        }

        [Test]
        public void Execute_TwoColumns_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", "1", 10 }, new object[] { "ALPHA", "1", 20 }, new object[] { "beta", "2", 30 }, new object[] { "ALPHA", "2", 40 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            var lowerCase = new SinglePredication(new TextLowerCase(false), new ColumnOrdinalIdentifier(0));
            var upperCase = new AndCombinationPredication(new List<IPredication>()
                {
                    new SinglePredication(new TextUpperCase(false), new ColumnOrdinalIdentifier(0)),
                    new SinglePredication(new TextEqual(false, new LiteralScalarResolver<string>("1")), new ColumnOrdinalIdentifier(1)),
                });

            var grouping = new CaseGrouping(new IPredication[] { lowerCase, upperCase }, Context.None);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result.ElementAt(0).Value.Rows.Count, Is.EqualTo(2));
            Assert.That(result.ElementAt(1).Value.Rows.Count, Is.EqualTo(1));
            Assert.That(result.ElementAt(2).Value.Rows.Count, Is.EqualTo(1));
        }

        [Test]
        public void Execute_TwoColumnsWithContext_ThreeGroups()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "alpha", "1", "1" }, new object[] { "ALPHA", "1", "1" }, new object[] { "beta", "2", "2" }, new object[] { "ALPHA", "2", "4" } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var context = new Context(null);
            var lowerCase = new SinglePredication(new TextLowerCase(false), new ColumnOrdinalIdentifier(0));
            var contextArgs = new ContextScalarResolverArgs(context, new ColumnOrdinalIdentifier(2));
            var upperCase = new AndCombinationPredication(new List<IPredication>()
                {
                    new SinglePredication(new TextUpperCase(false), new ColumnOrdinalIdentifier(0)),
                    new SinglePredication(new TextEqual(false, new ContextScalarResolver<string>(contextArgs)), new ColumnOrdinalIdentifier(1)),
                });

            var grouping = new CaseGrouping(new IPredication[] { lowerCase, upperCase }, context);

            var result = grouping.Execute(rs);
            Assert.That(result, Has.Count.EqualTo(3));
            Assert.That(result.ElementAt(0).Value.Rows.Count, Is.EqualTo(2));
            Assert.That(result.ElementAt(1).Value.Rows.Count, Is.EqualTo(1));
            Assert.That(result.ElementAt(2).Value.Rows.Count, Is.EqualTo(1));
        }
    }
}
