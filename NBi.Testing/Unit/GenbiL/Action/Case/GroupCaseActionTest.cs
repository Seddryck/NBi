using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class GroupCaseActionTest
    {
        protected GenerationState BuildState()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");

            return state;
        }

        [Test]
        public void Execute_ContentWithTwoGroupableRow_ContentReduced()
        {
            var state = BuildState();
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell1";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell1";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseCollection.Scope.Content.NewRow();
            thirdRow[0] = "firstCell1";
            thirdRow[1] = "secondCell3";
            thirdRow[2] = "thirdCell3";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdRow);
            state.TestCaseCollection.Scope.Content.AcceptChanges();

            var action = new GroupCaseAction(new[] { "secondColumn" });
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseCollection.Scope.Variables, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseCollection.Scope.Variables, Has.Member("secondColumn"));
            Assert.That(state.TestCaseCollection.Scope.Content.Columns["secondColumn"].Ordinal, Is.EqualTo(1));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.TypeOf<string[]>());
            var list = (state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"] as string[]).ToList();
            Assert.That(list, Has.Member("secondCell1"));
            Assert.That(list, Has.Member("secondCell2"));
            Assert.That(list, Has.Count.EqualTo(2));
            list = (state.TestCaseCollection.Scope.Content.Rows[1]["secondColumn"] as string[]).ToList();
            Assert.That(list, Has.Member("secondCell3"));
        }

        [Test]
        public void Execute_ContentWithTwoGroupableRowAtTheEnd_ContentReduced()
        {
            var state = BuildState();
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseCollection.Scope.Content.NewRow();
            thirdRow[0] = "firstCell2";
            thirdRow[1] = "secondCell3";
            thirdRow[2] = "thirdCell2";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdRow);
            state.TestCaseCollection.Scope.Content.AcceptChanges();

            var action = new GroupCaseAction(new[] { "secondColumn" });
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseCollection.Scope.Variables, Has.Count.EqualTo(3));
            Assert.That(state.TestCaseCollection.Scope.Variables, Has.Member("secondColumn"));
            Assert.That(state.TestCaseCollection.Scope.Content.Columns["secondColumn"].Ordinal, Is.EqualTo(1));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.TypeOf<string[]>());
            var list = (state.TestCaseCollection.Scope.Content.Rows[1]["secondColumn"] as string[]).ToList();
            Assert.That(list, Has.Member("secondCell2"));
            Assert.That(list, Has.Member("secondCell3"));
            Assert.That(list, Has.Count.EqualTo(2));
            list = (state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"] as string[]).ToList();
            Assert.That(list, Has.Member("secondCell1"));
        }

        [Test]
        public void Execute_ContentWithThreeGroupableRows_ContentReduced()
        {
            var state = BuildState();
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell1";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseCollection.Scope.Content.NewRow();
            thirdRow[0] = "firstCell1";
            thirdRow[1] = "secondCell3";
            thirdRow[2] = "thirdCell2";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdRow);
            state.TestCaseCollection.Scope.Content.AcceptChanges();

            var action = new GroupCaseAction(new[] { "secondColumn", "thirdColumn" });
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.TypeOf<string[]>());
            var list = (state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"] as string[]).ToList();
            Assert.That(list, Has.Member("secondCell1"));
            Assert.That(list, Has.Member("secondCell2"));
            Assert.That(list, Has.Member("secondCell3"));
            Assert.That(list, Has.Count.EqualTo(3));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["thirdColumn"], Is.TypeOf<string[]>());
            list = (state.TestCaseCollection.Scope.Content.Rows[0]["thirdColumn"] as string[]).ToList();
            Assert.That(list, Has.Member("thirdCell1"));
            Assert.That(list, Has.Member("thirdCell2"));
            Assert.That(list[1], Is.EqualTo("thirdCell2"));
            Assert.That(list[2], Is.EqualTo("thirdCell2"));
            Assert.That(list, Has.Count.EqualTo(3));

        }

        [Test]
        public void Execute_ContentWithFiveIdenticalRows_ContentReduced()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("secondColumn");
            
            state.TestCaseCollection.Scope.Variables.Add("firstColumn");
            state.TestCaseCollection.Scope.Variables.Add("secondColumn");
            
            Random rnd = new Random();
            for (int i = 0; i < 5; i++)
            {
                var row = state.TestCaseCollection.Scope.Content.NewRow();
                row[0] = rnd.Next(1, 100000);
                row[1] = "secondCell1";
                state.TestCaseCollection.Scope.Content.Rows.Add(row);
            }
            state.TestCaseCollection.Scope.Content.AcceptChanges();

            var action = new GroupCaseAction(new[] { "firstColumn" });
            action.Execute(state);
            Assert.That(state.TestCaseCollection.Scope.Content.Columns, Has.Count.EqualTo(2));

            Assert.That(state.TestCaseCollection.Scope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1"));
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["firstColumn"], Is.TypeOf<string[]>());
            var list = (state.TestCaseCollection.Scope.Content.Rows[0]["firstColumn"] as string[]).ToList();
            Assert.That(list, Has.Count.EqualTo(5));
        }
    }
}
