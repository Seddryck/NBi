using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NBi.Core.Scalar.Comparer;
using NBi.Extensibility;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Core.Testing.ResultSet.Lookup
{
    public class LookupMatchesAnalyzerTest
    {
        private DataTableResultSet BuildDataTable(object[] keys, object[] values)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("myKey");
            var valueCol = dt.Columns.Add("myValue");

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<object>(keyCol, keys[i]);
                dr.SetField<object>(valueCol, values[i]);
                dt.Rows.Add(dr);
            }

            return new DataTableResultSet(dt);
        }

        private DataTableResultSet BuildDataTable(object[] keys, object[] secondKeys, object[] values)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("zero");
            var secondKeyCol = dt.Columns.Add("one");
            var valueCol = dt.Columns.Add("two");

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField(keyCol, keys[i]);
                dr.SetField(secondKeyCol, secondKeys[i]);
                dr.SetField(valueCol, values[i]);
                dt.Rows.Add(dr);
            }

            return new DataTableResultSet(dt);
        }

        private ColumnMappingCollection BuildColumnMapping(int count, int shift = 0, ColumnType columnType = ColumnType.Text)
        {
            var mappings = new ColumnMappingCollection();
            for (int i = 0; i < count; i++)
                mappings.Add(new ColumnMapping(new ColumnOrdinalIdentifier(i + shift), new ColumnOrdinalIdentifier(i + shift), columnType));
            return mappings;
        }

        [Test]
        public void Execute_ReferenceLargerThanCandidateMatchingValue_NoViolation()
        {
            var candidate = BuildDataTable(new[] { "Key0", "Key1" }, [0, 1]);
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, [0, 1, 1]);

            var analyzer = new LookupMatchesAnalyzer(BuildColumnMapping(1), BuildColumnMapping(1,1));
            var violations = analyzer.Execute(candidate, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_ReferenceLargerThanCandidateMatchingValueWhenNoToleranceApplied_OneViolation()
        {
            var candidate = BuildDataTable(new[] { "Key0", "Key1" }, [0, 1]);
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, [0, 2, 1]);

            var analyzer = new LookupMatchesAnalyzer(BuildColumnMapping(1), BuildColumnMapping(1, 1, ColumnType.Numeric));
            var violations = analyzer.Execute(candidate, reference);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_ReferenceLargerThanCandidateMatchingValueWhenToleranceApplied_NoViolation()
        {
            var candidate = BuildDataTable(new[] { "Key0", "Key1" }, [0, 1]);
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, [0, 2, 1]);
            var tolerances = new Dictionary<IColumnIdentifier, Tolerance>() { { new ColumnIdentifierFactory().Instantiate("#1"), new NumericAbsoluteTolerance(1, SideTolerance.Both) } };
 
            var analyzer = new LookupMatchesAnalyzer(BuildColumnMapping(1), BuildColumnMapping(1, 1, ColumnType.Numeric), tolerances);
            var violations = analyzer.Execute(candidate, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_ReferenceLargerThanCandidateDuplicateKeys_NoViolation()
        {
            var candidate = BuildDataTable(new[] { "Key0", "Key1" }, [0, 1]);
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2", "Key1", "Key2" }, [0, 2, 3, 1, 3]);

            var analyzer = new LookupMatchesAnalyzer(BuildColumnMapping(1), BuildColumnMapping(1, 1));
            var violations = analyzer.Execute(candidate, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MissingKeyInReference_OneViolation()
        {
            var candidate = BuildDataTable(new[] { "Key0", "Key1" }, [0, 1]);
            var reference = BuildDataTable(new[] { "Key0", "Key2", "Key2", "Key0", "Key2" }, [0, 1, 1, 1, 1]);

            var analyzer = new LookupMatchesAnalyzer(BuildColumnMapping(1), BuildColumnMapping(1, 1));
            var violations = analyzer.Execute(candidate, reference);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_NotMatchValueInReference_OneViolation()
        {
            var candidate = BuildDataTable(new[] { "Key0", "Key1" }, [0, 1]);
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key1", "Key0", "Key2" }, [0, 2, 3, 4, 5]);

            var analyzer = new LookupMatchesAnalyzer(BuildColumnMapping(1), BuildColumnMapping(1, 1));
            var violations = analyzer.Execute(candidate, reference);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_MultipleKeysreferenceLargerThanCandidateDuplicateKeys_NoViolation()
        {
            var candidate = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, [0, 1]);
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar", "Bar" }, [0, 1, 2]);

            var referencer = new LookupMatchesAnalyzer(BuildColumnMapping(2), BuildColumnMapping(1, 2));
            var violations = referencer.Execute(candidate, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        

        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000)]
        [Retry(3)]
        [Parallelizable(ParallelScope.Self)]
        public void Execute_LargeVolumeReference_Fast(int maxItem)
        {
            var candidate = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, [1, 2, 3]);
            var dt = new DataTable();
            var idColumn = dt.Columns.Add("id");
            var valueColumn = dt.Columns.Add("value");
            var randomizer = new Random();
            for (int i = 0; i < maxItem; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<object>(idColumn, i);
                dr.SetField<object>(valueColumn, randomizer.Next().ToString());
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            var reference = new DataTableResultSet(dt);

            var mappingKey = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("two"), new ColumnNameIdentifier("id"), ColumnType.Numeric)
            };
            var mappingValue = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("one"), new ColumnNameIdentifier("value"), ColumnType.Text)
            };
            var analyzer = new LookupMatchesAnalyzer(mappingKey, mappingValue);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            analyzer.Execute(candidate, reference);
            stopWatch.Stop();
            Assert.That(stopWatch.Elapsed.TotalSeconds, Is.LessThan(20));
        }

        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000)]
        [Retry(3)]
        [Parallelizable(ParallelScope.Self)]
        public void Execute_LargeVolumeCandidate_Fast(int maxItem)
        {
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, [1, 2, 3]);
            var dt = new DataTable();
            var idColumn = dt.Columns.Add("id");
            var valueColumn = dt.Columns.Add("value");
            var randomizer = new Random();
            for (int i = 0; i < maxItem; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<object>(idColumn, i);
                dr.SetField<object>(valueColumn, randomizer.Next().ToString());
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            var candidate = new DataTableResultSet(dt);

            var mappingKey = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("id"), new ColumnNameIdentifier("two"), ColumnType.Numeric)
            };
            var mappingValue = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("value"), new ColumnNameIdentifier("one"), ColumnType.Text)
            };
            var analyzer = new LookupMatchesAnalyzer(mappingKey, mappingValue);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var violations = analyzer.Execute(candidate, reference);
            stopWatch.Stop();
            Assert.That(stopWatch.Elapsed.TotalSeconds, Is.LessThan(7));
        }
    }
}
