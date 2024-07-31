using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Case
{
    public class ReduceCaseActionTest
    {
        [Test]
        public void Display_LikeOneValue_CorrectString()
        {
            var action = new ReduceCaseAction(["foo", "bar"]);
            Assert.That(action.Display, Is.EqualTo("Reducing the length of groups for columns 'foo', 'bar'"));
        }

        [Test]
        public void Execute_ContentWithTwoGroupedRows_ContentReduced()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn", typeof(string[]));

            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = new [] {"thirdCell1", "thirdCell1", "thirdCell1" };
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);


            var action = new ReduceCaseAction(["thirdColumn"]);
            action.Execute(state);

            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["thirdColumn"], Is.TypeOf<string[]>());
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["thirdColumn"], Has.Member("thirdCell1"));
            Assert.That((state.CaseCollection.CurrentScope.Content.Rows[0]["thirdColumn"] as Array)!.Length, Is.EqualTo(1));
        }

        [Test]
        public void Execute_ContentWithTwoGroupedRowsForTwoColumns_ContentReduced()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn", typeof(string[]));
            state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn", typeof(string[]));

            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = new [] { "secondCell1", "secondCell1", "secondCell2" };
            firstRow[2] = new [] { "thirdCell1", "thirdCell1", "thirdCell1" };
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);


            var action = new ReduceCaseAction(["thirdColumn", "secondColumn"]);
            action.Execute(state);

            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(1));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["thirdColumn"], Is.TypeOf<string[]>());
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["thirdColumn"], Has.Member("thirdCell1"));
            Assert.That((state.CaseCollection.CurrentScope.Content.Rows[0]["thirdColumn"] as Array)!.Length, Is.EqualTo(1));

            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["secondColumn"], Is.TypeOf<string[]>());
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["secondColumn"], Has.Member("secondCell1"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["secondColumn"], Has.Member("secondCell2"));
            Assert.That((state.CaseCollection.CurrentScope.Content.Rows[0]["secondColumn"] as Array)!.Length, Is.EqualTo(2));
        }

    }
}
