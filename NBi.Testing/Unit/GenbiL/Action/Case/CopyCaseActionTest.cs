using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class CopyCaseActionTest
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
        public void Execute_SimpleMaster_CopyIsEffectivelyDone()
        {
            var state = new GenerationState();
            var master = state.TestCaseSetCollection.Item("firstScope");
            master.Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            newRow[0] = "firstCell-firstScope";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(newRow);

            var action = new CopyCaseAction("firstScope", "secondScope");
            action.Execute(state);

            Assert.That(state.TestCaseSetCollection.ItemExists("secondScope"), Is.True);
            var copied = state.TestCaseSetCollection.Item("secondScope");

            for (int i = 0; i < master.Content.Rows.Count; i++)
                Assert.That(copied.Content.Rows[i].ItemArray, Is.EqualTo(master.Content.Rows[i].ItemArray));

            Assert.That(copied.Content.Rows, Has.Count.EqualTo(master.Content.Rows.Count));
        }

        [Test]
        public void Copy_SimpleMaster_CopyIsNotReferenceCopy()
        {
            var state = new GenerationState();
            var master = state.TestCaseSetCollection.Item("master");
            Load(master.Content, new string[] { "a11,a12", "a11,a22", "a21,a32" }, "alpha1,alpha2");

            var action = new CopyCaseAction("master", "copied");
            action.Execute(state);

            var copied = state.TestCaseSetCollection.Item("copied");
            master.Content.Clear();

            Assert.That(master.Content.Rows, Has.Count.EqualTo(0));
            Assert.That(copied.Content.Rows, Has.Count.GreaterThan(0));
        }

        [Test]
        public void Copy_SimpleMasterWithCopiedAlreadyLoaded_CopyIsNotAllowed()
        {
            var state = new GenerationState();
            var master = state.TestCaseSetCollection.Item("master");
            Load(master.Content, new string[] { "a11,a12", "a11,a22", "a21,a32" }, "alpha1,alpha2");

            var copied = state.TestCaseSetCollection.Item("copied");
            Load(copied.Content, new string[] { "b11,b12", "b11,b22" }, "beta1,beta2");

            var action = new CopyCaseAction("master", "copied");
            Assert.Throws<ArgumentException>(delegate { action.Execute(state); });
        }

        [Test]
        public void Execute_TwoScopesWithDifferentColumns_CurrentScopeHasMoreRowsAndNewColumn()
        {
            var state = new GenerationState();
            state.TestCaseSetCollection.Item("firstScope").Content.Columns.Add("firstColumn");
            var newRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            newRow[0] = "firstCell-firstScope";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(newRow);

            state.TestCaseSetCollection.Item("secondScope").Content.Columns.Add("secondColumn");
            var newRowBis = state.TestCaseSetCollection.Item("secondScope").Content.NewRow();
            newRowBis[0] = "firstCell-secondScope";
            state.TestCaseSetCollection.Item("secondScope").Content.Rows.Add(newRowBis);

            var action = new MergeCaseAction("secondScope");
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(2));

            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0].ItemArray[0], Is.EqualTo("firstCell-firstScope"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[0].IsNull(1), Is.True);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[1].ItemArray[1], Is.EqualTo("firstCell-secondScope"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows[1].IsNull(0), Is.True);
        }

    }
}
