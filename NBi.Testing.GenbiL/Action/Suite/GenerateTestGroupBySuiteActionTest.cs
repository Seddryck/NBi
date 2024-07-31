using NBi.GenbiL.Action.Suite;
using NBi.GenbiL.Stateful;
using NBi.GenbiL.Stateful.Tree;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Suite
{
    public class GenerateTestGroupBySuiteActionTest
    {
        protected virtual GenerationState BuildInitialState()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("group");
            state.CaseCollection.CurrentScope.Content.Columns.Add("subgroup");
            state.CaseCollection.CurrentScope.Content.Columns.Add("dimension");
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow.ItemArray = new[] { "a", "b", "c" };
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow.ItemArray = new[] { "a", "d", "e" };
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);
            state.Templates.Add("<test name='$dimension$'/>");
            return state;
        }

        [Test]
        public void Execute_SimpleDataTable_ParentGroupGenerated()
        {
            var state = BuildInitialState();
            var action = new GenerateTestGroupBySuiteAction(@"$group$|$subgroup$");
            action.Execute(state);

            Assert.That(state.Suite.Children, Has.Count.EqualTo(1));
            var group = state.Suite.Children[0];
            Assert.That(group, Is.TypeOf<GroupNode>());
            Assert.That(group.Parent, Is.EqualTo(group.Root));
            Assert.That(group.Name, Is.EqualTo("a"));
        }

        [Test]
        public void Execute_SimpleDataTable_SubGroupsGenerated()
        {
            var state = BuildInitialState();
            var action = new GenerateTestGroupBySuiteAction(@"$group$|$subgroup$");
            action.Execute(state);

            Assert.That(state.Suite.Children, Has.Count.EqualTo(1));
            var parentGroup = state.Suite.Children[0] as BranchNode;
            Assert.That(parentGroup!.Children, Has.Count.EqualTo(2));
            Assert.That(parentGroup.Children, Has.All.TypeOf<GroupNode>());
            Assert.That(parentGroup.Children[0].Name, Is.EqualTo("b"));
            Assert.That(parentGroup.Children[1].Name, Is.EqualTo("d"));
        }

        [Test]
        public void Execute_SimpleDataTable_TestsGenerated()
        {
            var state = BuildInitialState();
            var action = new GenerateTestGroupBySuiteAction(@"$group$|$subgroup$");
            action.Execute(state);

            Assert.That(state.Suite.Children, Has.Count.EqualTo(1));
            var parentGroup = state.Suite.Children[0] as BranchNode;
            foreach (var childGroup in parentGroup!.Children.Cast<GroupNode>())
            {
                Assert.That(childGroup.Children, Has.Count.EqualTo(1));
                Assert.That(childGroup.Children, Has.All.TypeOf<TestNode>());
                Assert.That(childGroup.Children[0].Name, Is.EqualTo("c").Or.EqualTo("e"));
            }
        }
    }
}
