using NBi.Core;
using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Alteration.Lookup;
using NBi.Core.ResultSet.Alteration.Lookup.Strategies.Missing;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.ResultSet.Resolver;
using NBi.Extensibility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Alteration.Lookup
{
    public class LookupReplaceEngineTest
    {
        [Test]
        public void Execute_AllLookupFound_CorrectReplacement()
        {
            var candidate = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { 1, "A", 100 },
                        [2, "B", 101],
                        [3, "A", 125],
                        [4, "B", 155]
                    }
                )).Execute();

            var reference = new ObjectsResultSetResolver(
                    new ObjectsResultSetResolverArgs(
                        new[] {
                            new object[] { "A", "alpha" },
                            ["B", "beta"],
                        }
                    ));

            var engine = new LookupReplaceEngine(
                    new LookupReplaceArgs( 
                        reference, 
                        new ColumnMapping(new ColumnOrdinalIdentifier(1), new ColumnOrdinalIdentifier(0), ColumnType.Text),
                        new ColumnOrdinalIdentifier(1)
                ));

            var result = engine.Execute(candidate);
            Assert.That(result.ColumnCount, Is.EqualTo(3));
            Assert.That(result.Rows.Count, Is.EqualTo(4));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain("alpha"));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain("beta"));
            Assert.That(result.Rows.Select(x => x[1] as string).Where(x => x != "alpha" && x != "beta"), Is.Empty); 
        }

        [Test]
        public void Execute_AllLookupFoundSwitchingFromTextToNumeric_CorrectReplacement()
        {
            var candidate = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { 1, "A", 100 },
                        [2, "B", 101],
                        [3, "A", 125],
                        [4, "B", 155]
                    }
                )).Execute();

            var reference =  new ObjectsResultSetResolver(
                    new ObjectsResultSetResolverArgs(
                        new[] {
                            new object[] { "A", 10.2 },
                            ["B", 21.1],
                        }
                    ));

            var engine = new LookupReplaceEngine(
                    new LookupReplaceArgs(
                        reference,
                        new ColumnMapping(new ColumnOrdinalIdentifier(1), new ColumnOrdinalIdentifier(0), ColumnType.Text),
                        new ColumnOrdinalIdentifier(1)
                ));

            var result = engine.Execute(candidate);
            Assert.That(result.ColumnCount, Is.EqualTo(3));
            Assert.That(result.Rows.Count, Is.EqualTo(4));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain(10.2));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain(21.1));
            Assert.That(result.Rows.Select(x => Convert.ToDecimal(x[1])).Where(x => x != 10.2m && x != 21.1m), Is.Empty);
        }

        [Test]
        public void ExecuteWithFailureStretegy_OneLookupMissing_ExceptionThrown()
        {
            var candidate = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { 1, "A", 100 },
                        [2, "B", 101],
                        [3, "A", 125],
                        [4, "C", 155]
                    }
                )).Execute();

            var reference = 
                new ObjectsResultSetResolver(
                    new ObjectsResultSetResolverArgs(
                        new[] {
                            new object[] { "A", "alpha" },
                            ["B", "beta"],
                        }
                ));

            var engine = new LookupReplaceEngine(
                    new LookupReplaceArgs(
                        reference,
                        new ColumnMapping(new ColumnOrdinalIdentifier(1), new ColumnOrdinalIdentifier(0), ColumnType.Text),
                        new ColumnOrdinalIdentifier(1),
                        new FailureMissingStrategy()
                ));

            var ex = Assert.Throws<NBiException>(() => engine.Execute(candidate));
            Assert.That(ex!.Message, Does.Contain("'C'"));
        }

        [Test]
        public void ExecuteWithDefaultValueStrategy_OneLookupMissing_DefaultValueApplied()
        {
            var candidate = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { 1, "A", 100 },
                        [2, "B", 101],
                        [3, "A", 125],
                        [4, "C", 155]
                    }
                )).Execute();

            var reference = 
                new ObjectsResultSetResolver(
                    new ObjectsResultSetResolverArgs(
                        new[] {
                            new object[] { "A", "alpha" },
                            ["B", "beta"],
                        }
                ));

            var engine = new LookupReplaceEngine(
                    new LookupReplaceArgs(
                        reference,
                        new ColumnMapping(new ColumnOrdinalIdentifier(1), new ColumnOrdinalIdentifier(0), ColumnType.Text),
                        new ColumnOrdinalIdentifier(1),
                        new DefaultValueMissingStrategy("omega")
                ));

            var result = engine.Execute(candidate);
            Assert.That(result.Rows.Count, Is.EqualTo(4));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain("alpha"));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain("beta"));
            var otherValues = result.Rows.Select(x => x[1] as string).Where(x => x != "alpha" && x != "beta");
            Assert.That(otherValues, Is.Not.Empty);
            Assert.That(otherValues, Does.Contain("omega"));
        }

        [Test]
        public void ExecuteWithOriginalValueStrategy_OneLookupMissing_OriginalValueApplied()
        {
            var candidate = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { 1, "A", 100 },
                        [2, "B", 101],
                        [3, "A", 125],
                        [4, "C", 155]
                    }
                )).Execute();

            var reference = 
                new ObjectsResultSetResolver(
                    new ObjectsResultSetResolverArgs(
                        new[] {
                            new object[] { "A", "alpha" },
                            ["B", "beta"],
                        }
                ));

            var engine = new LookupReplaceEngine(
                    new LookupReplaceArgs(
                        reference,
                        new ColumnMapping(new ColumnOrdinalIdentifier(1), new ColumnOrdinalIdentifier(0), ColumnType.Text),
                        new ColumnOrdinalIdentifier(1),
                        new OriginalValueMissingStrategy()
                ));

            var result = engine.Execute(candidate);
            Assert.That(result.Rows.Count, Is.EqualTo(4));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain("alpha"));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain("beta"));
            var otherValues = result.Rows.Select(x => x[1] as string).Where(x => x != "alpha" && x != "beta");
            Assert.That(otherValues, Is.Not.Empty);
            Assert.That(otherValues, Does.Contain("C"));
        }

        [Test]
        public void ExecuteWithDiscardRowStrategy_OneLookupMissing_LessRowsReturned()
        {
            var candidate = new ObjectsResultSetResolver(
                new ObjectsResultSetResolverArgs(
                    new[] {
                        new object[] { 1, "A", 100 },
                        [2, "B", 101],
                        [3, "A", 125],
                        [4, "C", 155]
                    }
                )).Execute();

            var reference =
                new ObjectsResultSetResolver(
                    new ObjectsResultSetResolverArgs(
                        new[] {
                            new object[] { "A", "alpha" },
                            ["B", "beta"],
                        }
                ));

            var engine = new LookupReplaceEngine(
                    new LookupReplaceArgs(
                        reference,
                        new ColumnMapping(new ColumnOrdinalIdentifier(1), new ColumnOrdinalIdentifier(0), ColumnType.Text),
                        new ColumnOrdinalIdentifier(1),
                        new DiscardRowMissingStrategy()
                ));

            var result = engine.Execute(candidate);
            Assert.That(result.Rows.Count, Is.EqualTo(3));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain("alpha"));
            Assert.That(result.Rows.Select(x => x[1]).Distinct(), Does.Contain("beta"));
            var otherValues = result.Rows.Select(x => x[1] as string).Where(x => x != "alpha" && x != "beta");
            Assert.That(otherValues, Is.Empty);
        }
    }
}
