﻿using NBi.Core.Calculation.Predicate;
using NBi.Core.Calculation.Predication;
using NBi.Core.Injection;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Duplication;
using NBi.Core.ResultSet.Resolver;
using NBi.Core.Scalar.Resolver;
using NBi.Core.Transformation;
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
        public void Execute_TimesEqualOne_CorrectDuplication()
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
        public void Execute_Predication_CorrectDuplication()
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
        public void Execute_TimesDependingOnRow_CorrectDuplication()
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
        public void Execute_OuputStatic_CorrectStatic()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var duplicator = new DuplicateEngine(
                new ServiceLocator(),
                new Context(null),
                new PredicationFactory().True,
                new LiteralScalarResolver<int>(1),
                new List<OutputArgs>() { new OutputValueArgs(new ColumnNameIdentifier("NewValue"), "Static Value") }
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Columns[3].ColumnName, Is.EqualTo("NewValue"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(6));
            Assert.That(newRs.Rows[0][3], Is.EqualTo(DBNull.Value));
            Assert.That(newRs.Rows[1][3], Is.EqualTo("Static Value"));
            Assert.That(newRs.Rows[2][3], Is.EqualTo(DBNull.Value));
            Assert.That(newRs.Rows[3][3], Is.EqualTo("Static Value"));
            Assert.That(newRs.Rows[4][3], Is.EqualTo(DBNull.Value));
            Assert.That(newRs.Rows[5][3], Is.EqualTo("Static Value"));
        }

        [Test]
        public void Execute_OuputIndex_CorrectIndex()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 1, 2 }, new object[] { "Beta", 3, 2 }, new object[] { "Gamma", 5, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var duplicator = new DuplicateEngine(
                new ServiceLocator(),
                new Context(null),
                new PredicationFactory().True,
                new LiteralScalarResolver<int>(1),
                new List<OutputArgs>() { new OutputArgs(new ColumnNameIdentifier("Index"), OutputClass.Index) }
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Columns[3].ColumnName, Is.EqualTo("Index"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(6));
            Assert.That(newRs.Rows[0][3], Is.EqualTo(DBNull.Value));
            Assert.That(newRs.Rows[1][3], Is.EqualTo(0));
            Assert.That(newRs.Rows[2][3], Is.EqualTo(DBNull.Value));
            Assert.That(newRs.Rows[3][3], Is.EqualTo(0));
            Assert.That(newRs.Rows[4][3], Is.EqualTo(DBNull.Value));
            Assert.That(newRs.Rows[5][3], Is.EqualTo(0));
        }

        [Test]
        public void Execute_OutputTotal_CorrectTotal()
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
                new List<OutputArgs>() { new OutputArgs(new ColumnNameIdentifier("Total"), OutputClass.Total) }
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Columns[3].ColumnName, Is.EqualTo("Total"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(7));
            Assert.That(newRs.Rows[0][3], Is.EqualTo(DBNull.Value));
            Assert.That(newRs.Rows[1][3], Is.EqualTo(2));
            Assert.That(newRs.Rows[2][3], Is.EqualTo(2));
            Assert.That(newRs.Rows[3][3], Is.EqualTo(DBNull.Value));
            Assert.That(newRs.Rows[4][3], Is.EqualTo(2));
            Assert.That(newRs.Rows[5][3], Is.EqualTo(2));
            Assert.That(newRs.Rows[6][3], Is.EqualTo(DBNull.Value));
        }

        [Test]
        public void Execute_OutputIsOriginal_CorrectTotal()
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
                new List<OutputArgs>() { new OutputArgs(new ColumnNameIdentifier("IsOriginal"), OutputClass.IsOriginal) }
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Columns[3].ColumnName, Is.EqualTo("IsOriginal"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(7));
            for (int i = 0; i < newRs.Rows.Count; i++)
                Assert.That(newRs.Rows[i][3], Is.EqualTo(new[] { 0, 3, 6 }.Contains(i)));
        }

        [Test]
        public void Execute_OutputIsDuplicable_CorrectTotal()
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
                new List<OutputArgs>() { new OutputArgs(new ColumnNameIdentifier("IsDuplicable"), OutputClass.IsDuplicable) }
                );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Columns[3].ColumnName, Is.EqualTo("IsDuplicable"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(7));
            for (int i = 0; i < newRs.Rows.Count; i++)
                Assert.That(newRs.Rows[i][3], Is.EqualTo(!new[] { 6 }.Contains(i)));
        }

        [Test]
        public void Execute_OutputNativeScript_CorrectTotal()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 10, 2 }, new object[] { "Beta", 3, 3 }, new object[] { "Gamma", 30, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();

            var context = new Context(null);
            var duplicator = new DuplicateEngine(
               new ServiceLocator(),
               context,
               new PredicationFactory().Instantiate(
                        new PredicateFactory().Instantiate(
                            NBi.Core.Calculation.ComparerType.LessThan, ColumnType.Numeric, false, new LiteralScalarResolver<int>(20)
                        )
                    , new ColumnOrdinalIdentifier(1)),
                new ContextScalarResolver<int>(context, new ColumnOrdinalIdentifier(2)),
                new List<OutputArgs>() { new OutputScriptArgs(
                    new ServiceLocator(), context, new ColumnNameIdentifier("NewValue")
                    , LanguageType.Native, "#1 | numeric-to-divide(#2)")
                }
            );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(4));
            Assert.That(newRs.Columns[3].ColumnName, Is.EqualTo("NewValue"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(8));
            for (int i = 0; i < newRs.Rows.Count; i++)
                if (new[] { 0, 3, 7 }.Contains(i))
                    Assert.That(newRs.Rows[i][3], Is.EqualTo(DBNull.Value));
                else
                    Assert.That(new[] { 5, 1, 7 }.Contains(Convert.ToInt32(newRs.Rows[i][3])));
        }

        [Test]
        public void Execute_OutputNativeScriptOnExistingColumn_CorrectValue()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 10, 2 }, new object[] { "Beta", 3, 3 }, new object[] { "Gamma", 30, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[1].ColumnName = "Value";

            var context = new Context(null);
            var duplicator = new DuplicateEngine(
               new ServiceLocator(),
               context,
               new PredicationFactory().Instantiate(
                        new PredicateFactory().Instantiate(
                            NBi.Core.Calculation.ComparerType.LessThan, ColumnType.Numeric, false, new LiteralScalarResolver<int>(20)
                        )
                    , new ColumnOrdinalIdentifier(1)),
                new ContextScalarResolver<int>(context, new ColumnOrdinalIdentifier(2)),
                new List<OutputArgs>() { new OutputScriptArgs(
                    new ServiceLocator(), context, new ColumnNameIdentifier("Value")
                    , LanguageType.Native, "[Value] | numeric-to-divide(#2)")
                }
            );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(3));
            Assert.That(newRs.Columns[1].ColumnName, Is.EqualTo("Value"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(8));
            Assert.That(newRs.Rows[0][1], Is.EqualTo(10));
            Assert.That(newRs.Rows[1][1], Is.EqualTo(5));
            Assert.That(newRs.Rows[2][1], Is.EqualTo(5));
            Assert.That(newRs.Rows[3][1], Is.EqualTo(3));
            Assert.That(newRs.Rows[4][1], Is.EqualTo(1));
            Assert.That(newRs.Rows[5][1], Is.EqualTo(1));
            Assert.That(newRs.Rows[6][1], Is.EqualTo(1));
            Assert.That(newRs.Rows[7][1], Is.EqualTo(30));
        }

        [Test]
        public void Execute_OutputNCalcScriptOnExistingColumnAndUsingOtherOuputs_CorrectValue()
        {
            var args = new ObjectsResultSetResolverArgs(new[] { new object[] { "Alpha", 10, 2 }, new object[] { "Beta", 3, 3 }, new object[] { "Gamma", 30, 7 } });
            var resolver = new ObjectsResultSetResolver(args);
            var rs = resolver.Execute();
            rs.Columns[1].ColumnName = "Value";

            var serviceLocator = new ServiceLocator();
            var context = new Context(null);
            
            var duplicator = new DuplicateEngine(
               serviceLocator,
               context,
               new PredicationFactory().Instantiate(
                        new PredicateFactory().Instantiate(
                            NBi.Core.Calculation.ComparerType.LessThan, ColumnType.Numeric, false, new LiteralScalarResolver<int>(20)
                        )
                    , new ColumnOrdinalIdentifier(1)),
                new ContextScalarResolver<int>(context, new ColumnOrdinalIdentifier(2)),
                new List<OutputArgs>() {
                    new OutputArgs(new ColumnNameIdentifier("Total"), OutputClass.Total),
                    new OutputArgs(new ColumnNameIdentifier("Index"), OutputClass.Index),
                    new OutputScriptArgs(
                        serviceLocator, context, new ColumnNameIdentifier("Value")
                        , LanguageType.NCalc, "[Value]/[Total]*([Index]+1)"
                    )
                }
            );
            var newRs = duplicator.Execute(rs);

            Assert.That(newRs.Columns.Count, Is.EqualTo(5));
            Assert.That(newRs.Columns[1].ColumnName, Is.EqualTo("Value"));
            Assert.That(newRs.Rows.Count, Is.EqualTo(8));
            Assert.That(newRs.Rows[0][1], Is.EqualTo(10));
            Assert.That(newRs.Rows[1][1], Is.EqualTo(5));
            Assert.That(newRs.Rows[2][1], Is.EqualTo(10));
            Assert.That(newRs.Rows[3][1], Is.EqualTo(3));
            Assert.That(newRs.Rows[4][1], Is.EqualTo(1));
            Assert.That(newRs.Rows[5][1], Is.EqualTo(2));
            Assert.That(newRs.Rows[6][1], Is.EqualTo(3));
            Assert.That(newRs.Rows[7][1], Is.EqualTo(30));
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
                        NBi.Core.Calculation.ComparerType.LessThan, ColumnType.Numeric, false, new LiteralScalarResolver<int>(count / 2)
                    ), new ColumnNameIdentifier("a")),
                new LiteralScalarResolver<int>(1),
                new List<OutputArgs>() { new OutputArgs(new ColumnNameIdentifier("Index"), OutputClass.Index) }
                );
            duplicator.Execute(rs);
            stopWatch.Stop();

            Assert.That(stopWatch.ElapsedMilliseconds, Is.LessThanOrEqualTo(5000));
        }
    }
}
