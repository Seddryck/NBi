using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
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
    public class LookupExistsAnalyzerTest
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

        private IResultSet BuildDataTable(object[] keys, object[] secondKeys, object[] values)
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

        private ColumnMappingCollection BuildColumnMapping(int count, int shift = 0)
        {
            var mappings = new ColumnMappingCollection();
            for (int i = 0; i < count; i++)
                mappings.Add(new ColumnMapping(new ColumnOrdinalIdentifier(i), new ColumnOrdinalIdentifier(i + shift), ColumnType.Text));
            return mappings;
        }

        [Test]
        public void ExecuteReferenceLargerThanCandidate_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new object[] { 0, 1 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new object[] { 1, 1, 1 });

            var referencer = new LookupExistsAnalyzer(BuildColumnMapping(1));
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void ExecuteReferenceLargerThanCandidateDuplicateKeys_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new object[] { 0, 1 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2", "Key1", "Key2" }, new object[] { 1, 1, 1, 1, 1 });

            var referencer = new LookupExistsAnalyzer(BuildColumnMapping(1));
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MissingItem_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new object[] { 0, 1 });
            var reference = BuildDataTable(new[] { "Key0", "Key2", "Key2", "Key0", "Key2" }, new object[] { 1, 1, 1, 1, 1 });

            var referencer = new LookupExistsAnalyzer(BuildColumnMapping(1));
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_MultipleKeysreferenceLargerThanCandidateDuplicateKeys_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar", "Bar" }, new object[] { 1, 2, 3 });

            var referencer = new LookupExistsAnalyzer(BuildColumnMapping(2));
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MultipleKeysMissingItem_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var reference = BuildDataTable(new[] { "Key0" }, new[] { "Foo" }, new object[] { 1 });

            var referencer = new LookupExistsAnalyzer(BuildColumnMapping(2));
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_MultipleKeysPermuteValueColumn_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar",  "Fie" }, new object[] { 1, 2, 3 });
            reference.GetColumn(2)?.Move(0);

            var referencer = new LookupExistsAnalyzer(BuildColumnMapping(2, 1));
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MultipleKeysPermuteValueColumnOneMissingreference_OneViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar", "Bar" }, new object[] { 1, 2, 2 });
            var reference = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            reference.GetColumn(2)?.Move(0);

            var referencer = new LookupExistsAnalyzer(BuildColumnMapping(2, 1));
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_MultipleKeysPermuteKeyColumns_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar", "Fie" }, new object[] { 1, 2, 3 });
            reference.GetColumn(1)?.Move(0);

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnOrdinalIdentifier(0), new ColumnOrdinalIdentifier(1), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(1), new ColumnOrdinalIdentifier(0), ColumnType.Text)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MultipleKeysPermuteKeyColumnsOneMissingreference_OneViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar", "Fie" }, new object[] { 1, 2, 3 });
            var reference = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            reference.GetColumn(1)?.Move(0);

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnOrdinalIdentifier(0), new ColumnOrdinalIdentifier(1), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(1), new ColumnOrdinalIdentifier(0), ColumnType.Text)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_DuplicatedKeyColumns_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new object[] { 1, 2, 3 });

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnOrdinalIdentifier(0), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(1), ColumnType.Text)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_DuplicatedKeyColumnsOnBothSide_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 2 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new object[] { 1, 2, 3 });

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnOrdinalIdentifier(0), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(1), ColumnType.Text)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_DuplicatedKeyColumnsOnBothSideMixingType_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new [] { "0.000", "1.0", "2" });

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnOrdinalIdentifier(0), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(1), ColumnType.Text),
                new ColumnMapping(new ColumnOrdinalIdentifier(2), ColumnType.Numeric)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }


        [Test]
        public void Execute_NamedColumns_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new[] { "0.000", "1.0", "2" });

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("zero"), ColumnType.Text),
                new ColumnMapping(new ColumnNameIdentifier("one"), ColumnType.Text),
                new ColumnMapping(new ColumnNameIdentifier("two"), ColumnType.Numeric)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_NamedColumnsShuffle_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new[] { "0.000", "1.0", "2" });
            reference.GetColumn("two")?.Move(1);

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("zero"), ColumnType.Text),
                new ColumnMapping(new ColumnNameIdentifier("one"), ColumnType.Text),
                new ColumnMapping(new ColumnNameIdentifier("two"), ColumnType.Numeric)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count, Is.EqualTo(0));
        }

        [Test]
        public void Execute_RenamedColumnsShuffle_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new[] { "0.000", "1.0", "2" });
            reference.GetColumn("two")?.Move(1);
            reference.GetColumn("two")?.Rename("myColumn");

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("zero"), ColumnType.Text),
                new ColumnMapping(new ColumnNameIdentifier("one"), ColumnType.Text),
                new ColumnMapping(new ColumnNameIdentifier("two"), new ColumnNameIdentifier("myColumn"), ColumnType.Numeric)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count, Is.EqualTo(0));
        }

        [Test]
        public void Execute_MixNameAndOrdinal_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new[] { "0.000", "1.0", "2" });
            reference.GetColumn("two")?.Move(1);
            reference.GetColumn("two")?.Rename("myColumn");

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("zero"), ColumnType.Text),
                new ColumnMapping(new ColumnNameIdentifier("one"), ColumnType.Text),
                new ColumnMapping(new ColumnNameIdentifier("two"), new ColumnNameIdentifier("myColumn"), ColumnType.Numeric)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var violations = referencer.Execute(child, reference);
            Assert.That(violations.Count, Is.EqualTo(0));
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
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 1, 2, 3 });
            var dt = new DataTable();
            var column = dt.Columns.Add("two");
            for (int i = 0; i < maxItem; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<object>(column, i);
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            var reference = new DataTableResultSet(dt);

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("two"), new ColumnNameIdentifier("two"), ColumnType.Numeric)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            referencer.Execute(child, reference);
            stopWatch.Stop();
            Assert.That(stopWatch.Elapsed.TotalSeconds, Is.LessThan(30));
        }

        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(1000000)]
        [Retry(3)]
        [Parallelizable(ParallelScope.Self)]
        public void Execute_LargeVolumeChild_Fast(int maxItem)
        {
            var reference = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 1, 2, 3 });
            var dt = new DataTable();
            var column = dt.Columns.Add("two");
            for (int i = 0; i < maxItem; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<object>(column, i);
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            var child = new DataTableResultSet(dt);

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("two"), new ColumnNameIdentifier("two"), ColumnType.Numeric)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var violations = referencer.Execute(child, reference);
            stopWatch.Stop();
            Assert.That(stopWatch.Elapsed.TotalSeconds, Is.LessThan(10));
        }

        [Test]
        [TestCase(1000)]
        [TestCase(10000)]
        [TestCase(100000)]
        [TestCase(500000)]
        [Retry(3)]
        [Parallelizable(ParallelScope.Self)]
        public void Execute_LargeVolumeChildAndReference_Fast(int maxItem)
        {
            var dt = new DataTable();
            var column = dt.Columns.Add("one");
            for (int i = 0; i < maxItem; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<object>(column, i);
                dt.Rows.Add(dr);
            }
            dt.AcceptChanges();
            var child = new DataTableResultSet(dt);
            var reference = child.Clone();

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping(new ColumnNameIdentifier("one"), new ColumnNameIdentifier("one"), ColumnType.Numeric)
            };
            var referencer = new LookupExistsAnalyzer(mapping);
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            referencer.Execute(child, reference);
            stopWatch.Stop();
            Assert.That(stopWatch.Elapsed.TotalSeconds, Is.LessThan(10));
        }
    }
}
