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

namespace NBi.GenbiL.Testing.Action.Case
{
    public class CrossVectorCaseActionTest
    {
        [Test]
        public void Execute_VectorWithTwoValues_OriginalSetDoubled()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);


            var action = new CrossVectorCaseAction(state.CaseCollection.CurrentScopeName, "fourthColumn", ["Hello", "World"]);
            action.Execute(state);
            Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(4));
            Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[3], Is.EqualTo("fourthColumn"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(4));
        }

        [Test]
        public void Execute_VectorAndCellsWithArray_NoSpecificIssue()
        {
            var state = new GenerationState();
            state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1.1/firstCell1.2" ;
            state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell2.1/firstCell2.2";
            state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var splitAction = new SplitCaseAction(["firstColumn"], "/");
            splitAction.Execute(state);

            var action = new CrossVectorCaseAction(state.CaseCollection.CurrentScopeName, "helloColumn", ["Hello"]);
            action.Execute(state);
            Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[1], Is.EqualTo("helloColumn"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(2));
        }



        [Test]
        public void Display_SecondAndThirdColumns_CorrectMessage()
        {
            var action = new CrossVectorCaseAction("initialSet", "vector", ["Hello", "World"]);
            Assert.That(action.Display, Is.EqualTo("Crossing set of test-cases 'initialSet' with vector 'vector' defined as 'Hello', 'World'"));
        }
    }
}
