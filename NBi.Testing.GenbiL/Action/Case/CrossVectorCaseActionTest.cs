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
    public class CrossVectorCaseActionTest
    {
        [Test]
        public void Execute_VectorWithTwoValues_OriginalSetDoubled()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(secondRow);


            var action = new CrossVectorCaseAction(state.TestCaseCollection.CurrentScopeName, "fourthColumn", new [] {"Hello", "World"});
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(4));
            Assert.That(state.TestCaseCollection.CurrentScope.Variables.ToArray()[3], Is.EqualTo("fourthColumn"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(4));
        }

        [Test]
        public void Execute_VectorAndCellsWithArray_NoSpecificIssue()
        {
            var state = new GenerationState();
            state.TestCaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
            var firstRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            firstRow[0] = "firstCell1.1/firstCell1.2" ;
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.CurrentScope.Content.NewRow();
            secondRow[0] = "firstCell2.1/firstCell2.2";
            state.TestCaseCollection.CurrentScope.Content.Rows.Add(secondRow);

            var splitAction = new SplitCaseAction(new[] { "firstColumn" }, "/");
            splitAction.Execute(state);

            var action = new CrossVectorCaseAction(state.TestCaseCollection.CurrentScopeName, "helloColumn", new[] { "Hello" });
            action.Execute(state);
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(2));
            Assert.That(state.TestCaseCollection.CurrentScope.Variables.ToArray()[1], Is.EqualTo("helloColumn"));
            Assert.That(state.TestCaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(2));
        }



        [Test]
        public void Display_SecondAndThirdColumns_CorrectMessage()
        {
            var action = new CrossVectorCaseAction("initialSet", "vector", new[] { "Hello", "World" });
            Assert.That(action.Display, Is.EqualTo("Crossing set of test-cases 'initialSet' with vector 'vector' defined as 'Hello', 'World'"));
        }
    }
}
