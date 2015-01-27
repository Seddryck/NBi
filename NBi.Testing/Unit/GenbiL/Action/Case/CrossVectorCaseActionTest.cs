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
    public class CrossVectorCaseActionTest
    {
        [Test]
        public void Execute_VectorWithTwoValues_OriginalSetDoubled()
        {
            var state = new GenerationState();
            state.TestCaseSetCollection.Scope.Content.Columns.Add("firstColumn");
            state.TestCaseSetCollection.Scope.Content.Columns.Add("secondColumn");
            state.TestCaseSetCollection.Scope.Content.Columns.Add("thirdColumn");
            var firstRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            firstRow[0] = "firstCell1";
            firstRow[1] = "secondCell1";
            firstRow[2] = "thirdCell1";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseSetCollection.Scope.Content.NewRow();
            secondRow[0] = "firstCell2";
            secondRow[1] = "secondCell2";
            secondRow[2] = "thirdCell2";
            state.TestCaseSetCollection.Scope.Content.Rows.Add(secondRow);


            var action = new CrossVectorCaseAction(state.TestCaseSetCollection.CurrentScopeName, "fourthColumn", new [] {"Hello", "World"});
            action.Execute(state);
            Assert.That(state.TestCaseSetCollection.Scope.Content.Columns, Has.Count.EqualTo(4));
            Assert.That(state.TestCaseSetCollection.Scope.Variables[3], Is.EqualTo("fourthColumn"));
            Assert.That(state.TestCaseSetCollection.Scope.Content.Rows, Has.Count.EqualTo(4));
        }



        [Test]
        public void Display_SecondAndThirdColumns_CorrectMessage()
        {
            var action = new CrossVectorCaseAction("initialSet", "vector", new[] { "Hello", "World" });
            Assert.That(action.Display, Is.EqualTo("Crossing test cases set 'initialSet' with vector 'vector' defined as 'Hello', 'World'"));
        }
    }
}
