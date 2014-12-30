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
    public class CrossCaseActionTest
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
        public void CrossFull_ThreeTimesTwo_SixRowsFiveColumns()
        {
            var state = new GenerationState();
            var tc1 = state.TestCaseSetCollection.Item("alpha");
            Load(tc1.Content, new string[] { "a11,a12", "a21,a22", "a31,a32" }, "alpha1,alpha2");
            var tc2 = state.TestCaseSetCollection.Item("beta");
            Load(tc2.Content, new string[] { "b11,b12,b13", "b21,b22,b23" }, "beta1,beta2,beta3");

            state.TestCaseSetCollection.SetFocus("gamma");
            var action = new CrossFullCaseAction("alpha", "beta");
            action.Execute(state);

            var focus = state.TestCaseSetCollection.Scope;

            Assert.That(focus.Content.Rows, Has.Count.EqualTo(6));
            Assert.That(focus.Content.Columns, Has.Count.EqualTo(5));
        }

        [Test]
        public void CrossFull_ThreeTimesTwoOnItself_SixRowsFiveColumns()
        {
            var state = new GenerationState();
            var tc1 = state.TestCaseSetCollection.Item("alpha");
            Load(tc1.Content, new string[] { "a11,a12", "a21,a22", "a31,a32" }, "alpha1,alpha2");
            var tc2 = state.TestCaseSetCollection.Item("beta");
            Load(tc2.Content, new string[] { "b11,b12,b13", "b21,b22,b23" }, "beta1,beta2,beta3");
            state.TestCaseSetCollection.SetFocus("alpha");

            var action = new CrossFullCaseAction("alpha", "beta");
            action.Execute(state);
            //The focus hasn't moved ... so still on tc1
            Assert.That(tc1.Content.Rows, Has.Count.EqualTo(6));
            Assert.That(tc1.Content.Columns, Has.Count.EqualTo(5));
        }

        [Test]
        public void CrossFull_ThreeTimesTwoWithOneCommonColumnName_SixRowsFourColumns()
        {
            var state = new GenerationState();
            var tc1 = state.TestCaseSetCollection.Item("alpha");
            Load(tc1.Content, new string[] { "a11,a12", "a21,a22", "a31,a32" }, "alpha1,alpha2");
            var tc2 = state.TestCaseSetCollection.Item("beta");
            Load(tc2.Content, new string[] { "b11,b12,b13", "b21,b22,b23" }, "alpha1,beta2,beta3");

            state.TestCaseSetCollection.SetFocus("gamma");
            var action = new CrossFullCaseAction("alpha", "beta");
            action.Execute(state);
            var focus = state.TestCaseSetCollection.Scope;

            Assert.That(focus.Content.Rows, Has.Count.EqualTo(6));
            Assert.That(focus.Content.Columns, Has.Count.EqualTo(4));
        }

        [Test]
        public void CrossColumnMatching_ThreeTimesTwoOnMatchingColumn_FourRowsFourColumns()
        {
            var state = new GenerationState();
            var tc1 = state.TestCaseSetCollection.Item("alpha");
            Load(tc1.Content, new string[] { "a11,a12", "a21,a22", "a31,a32" }, "alpha1,alpha2");
            var tc2 = state.TestCaseSetCollection.Item("beta");
            Load(tc2.Content, new string[] { "a11,b12,b13", "a21,b22,b23", "a21,b32,b33", "a21,b42,b43", "a41,b52,b53" }, "alpha1,beta2,beta3");

            state.TestCaseSetCollection.SetFocus("gamma");
            var action = new CrossColumnMatchingCaseAction("alpha", "beta", "alpha1");
            action.Execute(state);
            var focus = state.TestCaseSetCollection.Scope;

            Assert.That(focus.Content.Rows, Has.Count.EqualTo(4));
            Assert.That(focus.Content.Columns, Has.Count.EqualTo(4));
        }

        [Test]
        public void CrossColumnMatching_ThreeTimesTwoOnMatchingColumnWithoutPrimaryKey_FiveRowsFourColumns()
        {
            var state = new GenerationState();
            var tc1 = state.TestCaseSetCollection.Item("alpha");
            Load(tc1.Content, new string[] { "a11,a12", "a11,a22", "a21,a32" }, "alpha1,alpha2");
            var tc2 = state.TestCaseSetCollection.Item("beta");
            Load(tc2.Content, new string[] { "a11,b12,b13", "a21,b22,b23", "a21,b32,b33", "a21,b42,b43", "a41,b52,b53" }, "alpha1,beta2,beta3");

            state.TestCaseSetCollection.SetFocus("gamma");
            var action = new CrossColumnMatchingCaseAction("alpha", "beta", "alpha1");
            action.Execute(state);
            var focus = state.TestCaseSetCollection.Scope;

            Assert.That(focus.Content.Rows, Has.Count.EqualTo(5));
            Assert.That(focus.Content.Columns, Has.Count.EqualTo(4));
        }


    }
}
