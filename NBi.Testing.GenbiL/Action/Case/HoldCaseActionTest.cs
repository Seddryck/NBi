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

namespace NBi.Testing.GenbiL.Action.Case
{
    public class HoldCaseActionTest
    {
        [Test]
        public void Execute_SecondColumn_ColumnHeld()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell";
            firstRow[1] = "secondCell";
            firstRow[2] = "thirdCell";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(firstRow);

            var action = new HoldCaseAction("secondColumn");
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(1));
            Assert.That(state.TestCaseCollection.CurrentScope.Variables.ToArray()[0], Is.EqualTo("secondColumn"));
        }


        [Test]
        public void Execute_SecondAndThirdColumns_ColumnsHeld()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell";
            firstRow[1] = "secondCell";
            firstRow[2] = "thirdCell";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(firstRow);

            var action = new HoldCaseAction(new List<string>() {"secondColumn", "firstColumn"});
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.CurrentScope.Variables, Has.Member("secondColumn"));
            Assert.That(state.TestCaseCollection.CurrentScope.Variables, Has.Member("firstColumn"));
        }

        [Test]
        public void Display_SecondColumn_CorrectMessage()
        {
            var action = new HoldCaseAction("secondColumn");
            Assert.That(action.Display, Is.EqualTo("Holding column 'secondColumn'"));
        }

        [Test]
        public void Display_SecondAndThirdColumns_CorrectMessage()
        {
            var action = new HoldCaseAction(new List<string>() {"secondColumn", "firstColumn"});
            Assert.That(action.Display, Is.EqualTo("Holding columns 'secondColumn', 'firstColumn'"));
        }
    }
}
