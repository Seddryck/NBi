using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predication;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Duplication;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Variable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Core.ResultSet.Alteration.Duplication
{
    public class DuplicateEngineTest
    {
        [Test]
        public void Execute_NoPredicationTimesEqualOne_CorrectDuplication()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var duplicator = new DuplicateEngine(
                new ServiceLocator(),
                new Context(null), 
                new PredicationFactory().True,
                new LiteralScalarResolver<int>(1),
                new List<OutputArgs>()
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(3));
            Assert.That(newRs.Rows.Count, Is.EqualTo(6));
            Assert.That(newRs.Rows[0][0], Is.EqualTo("Alpha"));
            Assert.That(newRs.Rows[1][0], Is.EqualTo("Alpha"));
            Assert.That(newRs.Rows[2][0], Is.EqualTo("Beta"));
            Assert.That(newRs.Rows[3][0], Is.EqualTo("Beta"));
            Assert.That(newRs.Rows[4][0], Is.EqualTo("Gamma"));
            Assert.That(newRs.Rows[5][0], Is.EqualTo("Gamma"));
        }

        [Test]
        public void Execute_PredicationTimesEqualOne_CorrectDuplication()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var duplicator = new DuplicateEngine(
                new ServiceLocator(),
                new Context(null),
                new PredicationFactory().Instantiate(new PredicateFactory().Instantiate(
                    NBi.Core.Calculation.ComparerType.LessThan, ColumnType.Numeric, false, 
                    new LiteralScalarResolver<int>(4), "en-us", StringComparison.OrdinalIgnoreCase, null
                    ), new ColumnOrdinalIdentifier(1)),
                new LiteralScalarResolver<int>(1),
                new List<OutputArgs>()
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(3));
            Assert.That(newRs.Rows.Count, Is.EqualTo(5));
            Assert.That(newRs.Rows[0][0], Is.EqualTo("Alpha"));
            Assert.That(newRs.Rows[1][0], Is.EqualTo("Alpha"));
            Assert.That(newRs.Rows[2][0], Is.EqualTo("Beta"));
            Assert.That(newRs.Rows[3][0], Is.EqualTo("Beta"));
            Assert.That(newRs.Rows[4][0], Is.EqualTo("Gamma"));
        }

        [Test]
        public void Execute_PredicationTimesDependingOnRow_CorrectDuplication()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 3 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var context = new Context(null);

            var duplicator = new DuplicateEngine(
                new ServiceLocator(),
                context,
                new PredicationFactory().Instantiate(
                        new PredicateFactory().Instantiate(
                            NBi.Core.Calculation.ComparerType.LessThan, ColumnType.Numeric, false, new LiteralScalarResolver<int>(4)
                        )
                    , new ColumnOrdinalIdentifier(1)),
                new ContextScalarResolver<int>(context, new ColumnOrdinalIdentifier(2)),
                new List<OutputArgs>()
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(3));
            Assert.That(newRs.Rows.Count, Is.EqualTo(8));
            Assert.That(newRs.Rows[0][0], Is.EqualTo("Alpha"));
            Assert.That(newRs.Rows[1][0], Is.EqualTo("Alpha"));
            Assert.That(newRs.Rows[2][0], Is.EqualTo("Alpha"));
            Assert.That(newRs.Rows[3][0], Is.EqualTo("Beta"));
            Assert.That(newRs.Rows[4][0], Is.EqualTo("Beta"));
            Assert.That(newRs.Rows[5][0], Is.EqualTo("Beta"));
            Assert.That(newRs.Rows[6][0], Is.EqualTo("Beta"));
            Assert.That(newRs.Rows[7][0], Is.EqualTo("Gamma"));
        }

        [Test]
        public void Execute_NoPredicationTimesEqualOneWithOutput_CorrectIndex()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var duplicator = new DuplicateEngine(
                new ServiceLocator(),
                new Context(null),
                new PredicationFactory().True,
                new LiteralScalarResolver<int>(1),
                new List<OutputArgs>() { new OutputArgs("Index", OutputValue.Index) }
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Columns[3].ColumnName, Is.EqualTo("Index"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(6));
            Assert.That(newRs.Rows[0][3], Is.EqualTo(0));
            Assert.That(newRs.Rows[1][3], Is.EqualTo(1));
            Assert.That(newRs.Rows[2][3], Is.EqualTo(0));
            Assert.That(newRs.Rows[3][3], Is.EqualTo(1));
            Assert.That(newRs.Rows[4][3], Is.EqualTo(0));
            Assert.That(newRs.Rows[5][3], Is.EqualTo(1));
        }

        [Test]
        public void Execute_NoPredicationTimesEqualOneWithOutput_CorrectTotal()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var context = new Context(null);
            var duplicator = new DuplicateEngine(
               new ServiceLocator(),
               context,
               new PredicationFactory().Instantiate(
                        new PredicateFactory().Instantiate(
                            NBi.Core.Calculation.ComparerType.LessThan, ColumnType.Numeric, false, new LiteralScalarResolver<int>(4)
                        )
                    , new ColumnOrdinalIdentifier(1)),
                new ContextScalarResolver<int>(context, new ColumnOrdinalIdentifier(2)),
                new List<OutputArgs>() { new OutputArgs("Total", OutputValue.Total) }
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Columns[3].ColumnName, Is.EqualTo("Total"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(7));
            Assert.That(newRs.Rows[0][3], Is.EqualTo(3));
            Assert.That(newRs.Rows[1][3], Is.EqualTo(3));
            Assert.That(newRs.Rows[2][3], Is.EqualTo(3));
            Assert.That(newRs.Rows[3][3], Is.EqualTo(3));
            Assert.That(newRs.Rows[4][3], Is.EqualTo(3));
            Assert.That(newRs.Rows[5][3], Is.EqualTo(3));
            Assert.That(newRs.Rows[6][3], Is.EqualTo(1));
        }

        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [Retry(3)]
        public void Execute_ManyTimes_Performances(int count)
        {
            var rows = new List<object[]>();
            for (int i = 0; i < count; i++)
                rows.Add(new object[] { i, i + 1 });

            var args = new ObjectsResultSetResolverArgs(rows.ToArray());
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[0].ColumnName = "a";
            rs.Columns[1].ColumnName = "b";

            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var duplicator = new DuplicateEngine(
                new ServiceLocator(),
                new Context(null),
                new PredicationFactory().Instantiate(new PredicateFactory().Instantiate(
                        NBi.Core.Calculation.ComparerType.LessThan, ColumnType.Numeric, false, new LiteralScalarResolver<int>(count/2)
                    ), new ColumnNameIdentifier("a")),
                new LiteralScalarResolver<int>(1),
                new List<OutputArgs>() { new OutputArgs("Index", OutputValue.Index) }
                );
            duplicator.Execute(rs);
            stopWatch.Stop();

            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThanOrEqualTo(5000));
        }
    }
}
