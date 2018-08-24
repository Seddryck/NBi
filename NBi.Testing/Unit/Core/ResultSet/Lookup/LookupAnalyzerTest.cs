using NBi.Core.ResultSet;
using NBi.Core.ResultSet.Lookup;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.Core.ResultSet.Lookup
{
    public class LookupAnalyzerTest
    {
        protected DataTable BuildDataTable(object[] keys, object[] values)
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

            return dt;
        }

        protected DataTable BuildDataTable(object[] keys, object[] secondKeys, object[] values)
        {
            var ds = new DataSet();
            var dt = ds.Tables.Add("myTable");

            var keyCol = dt.Columns.Add("zero");
            var secondKeyCol = dt.Columns.Add("one");
            var valueCol = dt.Columns.Add("two");

            for (int i = 0; i < keys.Length; i++)
            {
                var dr = dt.NewRow();
                dr.SetField<object>(keyCol, keys[i]);
                dr.SetField<object>(secondKeyCol, secondKeys[i]);
                dr.SetField<object>(valueCol, values[i]);
                dt.Rows.Add(dr);
            }

            return dt;
        }

        private ColumnMappingCollection BuildColumnMapping(int count, int shift = 0)
        {
            var mappings = new ColumnMappingCollection();
            for (int i = 0; i < count; i++)
                mappings.Add(new ColumnMapping($"#{i}", $"#{i + shift}", ColumnType.Text));
            return mappings;
        }

        [Test]
        public void Execute_ParentLargerThanChild_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new object[] { 0, 1 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new object[] { 1, 1, 1 });

            var referencer = new LookupAnalyzer(BuildColumnMapping(1));
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_ParentLargerThanChildDuplicateKeys_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new object[] { 0, 1 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key2", "Key1", "Key2" }, new object[] { 1, 1, 1, 1, 1 });

            var referencer = new LookupAnalyzer(BuildColumnMapping(1));
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MissingItem_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new object[] { 0, 1 });
            var parent = BuildDataTable(new[] { "Key0", "Key2", "Key2", "Key0", "Key2" }, new object[] { 1, 1, 1, 1, 1 });

            var referencer = new LookupAnalyzer(BuildColumnMapping(1));
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_MultipleKeysParentLargerThanChildDuplicateKeys_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar", "Bar" }, new object[] { 1, 2, 3 });

            var referencer = new LookupAnalyzer(BuildColumnMapping(2));
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MultipleKeysMissingItem_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var parent = BuildDataTable(new[] { "Key0" }, new[] { "Foo" }, new object[] { 1 });

            var referencer = new LookupAnalyzer(BuildColumnMapping(2));
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_MultipleKeysPermuteValueColumn_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar",  "Fie" }, new object[] { 1, 2, 3 });
            parent.Columns[2].SetOrdinal(0);

            var referencer = new LookupAnalyzer(BuildColumnMapping(2, 1));
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MultipleKeysPermuteValueColumnOneMissingParent_OneViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar", "Bar" }, new object[] { 1, 2, 2 });
            var parent = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            parent.Columns[2].SetOrdinal(0);

            var referencer = new LookupAnalyzer(BuildColumnMapping(2, 1));
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_MultipleKeysPermuteKeyColumns_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar", "Fie" }, new object[] { 1, 2, 3 });
            parent.Columns[1].SetOrdinal(0);

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping("#0", "#1", ColumnType.Text),
                new ColumnMapping("#1", "#0", ColumnType.Text)
            };
            var referencer = new LookupAnalyzer(mapping);
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MultipleKeysPermuteKeyColumnsOneMissingParent_OneViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key2" }, new[] { "Foo", "Bar", "Fie" }, new object[] { 1, 2, 3 });
            var parent = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            parent.Columns[1].SetOrdinal(0);

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping("#0", "#1", ColumnType.Text),
                new ColumnMapping("#1", "#0", ColumnType.Text)
            };
            var referencer = new LookupAnalyzer(mapping);
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(1));
        }

        [Test]
        public void Execute_DuplicatedKeyColumns_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1" }, new[] { "Foo", "Bar" }, new object[] { 0, 1 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new object[] { 1, 2, 3 });

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping("#0", "#0", ColumnType.Text),
                new ColumnMapping("#1", "#1", ColumnType.Text)
            };
            var referencer = new LookupAnalyzer(mapping);
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_DuplicatedKeyColumnsOnBothSide_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 2 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new object[] { 1, 2, 3 });

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping("#0", "#0", ColumnType.Text),
                new ColumnMapping("#1", "#1", ColumnType.Text)
            };
            var referencer = new LookupAnalyzer(mapping);
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_DuplicatedKeyColumnsOnBothSideMixingType_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new [] { "0.000", "1.0", "2" });

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping("#0", "#0", ColumnType.Text),
                new ColumnMapping("#1", "#1", ColumnType.Text),
                new ColumnMapping("#2", "#2", ColumnType.Numeric)
            };
            var referencer = new LookupAnalyzer(mapping);
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }


        [Test]
        public void Execute_NamedColumns_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new[] { "0.000", "1.0", "2" });

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping("zero", "zero", ColumnType.Text),
                new ColumnMapping("one", "one", ColumnType.Text),
                new ColumnMapping("two", "two", ColumnType.Numeric)
            };
            var referencer = new LookupAnalyzer(mapping);
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_NamedColumnsShuffle_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new[] { "0.000", "1.0", "2" });
            parent.Columns["two"].SetOrdinal(1);

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping("zero", "zero", ColumnType.Text),
                new ColumnMapping("one", "one", ColumnType.Text),
                new ColumnMapping("two", "two", ColumnType.Numeric)
            };
            var referencer = new LookupAnalyzer(mapping);
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_RenamedColumnsShuffle_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new[] { "0.000", "1.0", "2" });
            parent.Columns["two"].SetOrdinal(1);
            parent.Columns["two"].ColumnName = "myColumn";

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping("zero", "zero", ColumnType.Text),
                new ColumnMapping("one", "one", ColumnType.Text),
                new ColumnMapping("two", "myColumn", ColumnType.Numeric)
            };
            var referencer = new LookupAnalyzer(mapping);
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }

        [Test]
        public void Execute_MixNameAndIndex_NoViolation()
        {
            var child = BuildDataTable(new[] { "Key0", "Key1", "Key0" }, new[] { "Foo", "Bar", "Foo" }, new object[] { 0, 1, 0 });
            var parent = BuildDataTable(new[] { "Key0", "Key1", "Key1" }, new[] { "Foo", "Bar", "Bar" }, new[] { "0.000", "1.0", "2" });
            parent.Columns["two"].SetOrdinal(1);
            parent.Columns["two"].ColumnName = "myColumn";

            var mapping = new ColumnMappingCollection
            {
                new ColumnMapping("#0", "zero", ColumnType.Text),
                new ColumnMapping("#1", "one", ColumnType.Text),
                new ColumnMapping("#2", "myColumn", ColumnType.Numeric)
            };
            var referencer = new LookupAnalyzer(mapping);
            var violations = referencer.Execute(child, parent);
            Assert.That(violations.Count(), Is.EqualTo(0));
        }
    }
}
