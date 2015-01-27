using NBi.GenbiL.Stateful;
using NBi.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Stateful
{
    public class TestCaseSetCollectionTest
    {
        private void Load(DataTable table, string[] rows, string columnNames)
        {
            var columns = columnNames.Split(',');
            for (int i = 0; i < columns.Length; i++)
                table.Columns.Add(new DataColumn(columns[i]));

            foreach (var row in rows)
            {
                var newRow = table.NewRow();
                newRow.ItemArray = row.Split(',');
                table.Rows.Add(newRow);
            }

            table.AcceptChanges();
        }
        
        [Test]
        public void Item_EmptyCollection_ElementCreated()
        {
            var manager = new TestCaseSetCollectionState();
            var tc = manager.Item("alpha");
            Assert.That(tc, Is.Not.Null);
            Assert.That(tc, Is.TypeOf<TestCaseSetState>());
        }

        [Test]
        public void Item_EmptyCollectionWithNoName_ElementCreated()
        {
            var manager = new TestCaseSetCollectionState();
            var tc = manager.Item(string.Empty);
            Assert.That(tc, Is.Not.Null);
            Assert.That(tc, Is.TypeOf<TestCaseSetState>());
        }

        [Test]
        public void Item_GetTwice_SameElement()
        {
            var manager = new TestCaseSetCollectionState();
            var tc1 = manager.Item("alpha");
            var tc2 = manager.Item("alpha");

            Assert.That(tc1, Is.EqualTo(tc2));
        }

        [Test]
        public void Item_GetTwoDistinct_DifferentElements()
        {
            var manager = new TestCaseSetCollectionState();
            var tc1 = manager.Item("alpha");
            var tc2 = manager.Item("beta");

            Assert.That(tc1, Is.Not.EqualTo(tc2));
        }

        [Test]
        public void Item_NullName_DifferentElements()
        {
            var manager = new TestCaseSetCollectionState();
            var tc1 = manager.Item(null);

            Assert.That(tc1, Is.Not.Null);
        }

        [Test]
        public void Focus_TwoElementsCreatedSetFocusNeverCalled_FirstElement()
        {
            var manager = new TestCaseSetCollectionState();
            var tc1 = manager.Item("alpha");
            var tc2 = manager.Item("beta");

            var focus = manager.Scope;

            Assert.That(focus, Is.EqualTo(tc1));
        }

        [Test]
        public void Focus_TwoElementsCreatedSetFocusCalledForSecond_SecondElement()
        {
            var manager = new TestCaseSetCollectionState();
            var tc1 = manager.Item("alpha");
            var tc2 = manager.Item("beta");

            manager.SetFocus("beta");
            var focus = manager.Scope;

            Assert.That(focus, Is.EqualTo(tc2));
        }

        
    }
}
