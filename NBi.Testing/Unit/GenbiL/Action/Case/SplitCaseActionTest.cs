using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Parser.Valuable;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class SplitCaseActionTest
    {
        [Test]
        public void Execute_OneColumnToSplit_Correct()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("initialColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("otherColumn");
            state.TestCaseCollection.Scope.Variables.Add("initialColumn");
            state.TestCaseCollection.Scope.Variables.Add("otherColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "a-b-c";
            firstRow[1] = "other";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "a-b";
            secondRow[1] = "other";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseCollection.Scope.Content.NewRow();
            thirdRow[0] = "a-b-c-d";
            thirdRow[1] = "(none)";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdRow);
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["initialColumn"], Is.TypeOf<string>());


            var columns = new string[] { "initialColumn" };

            var action = new SplitCaseAction(columns, "-");
            action.Execute(state);
            var dataTable = state.TestCaseCollection.Scope.Content;
            Assert.That(dataTable.Columns, Has.Count.EqualTo(2));
            Assert.That(dataTable.Rows, Has.Count.EqualTo(3));

            Assert.That(dataTable.Rows[0]["otherColumn"], Is.TypeOf<string>());
            Assert.That(dataTable.Rows[0]["initialColumn"], Is.TypeOf<string[]>());
            
            Assert.That((dataTable.Rows[0]["initialColumn"] as Array).Length, Is.EqualTo(3));
            Assert.That((dataTable.Rows[1]["initialColumn"] as Array).Length, Is.EqualTo(2));
            Assert.That((dataTable.Rows[2]["initialColumn"] as Array).Length, Is.EqualTo(4));
        }

        [Test]
        public void Execute_TwoColumnsToSplit_Correct()
        {
            var state = new GenerationState();
            state.TestCaseCollection.Scope.Content.Columns.Add("initialColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("otherColumn");
            state.TestCaseCollection.Scope.Content.Columns.Add("thirdColumn");
            state.TestCaseCollection.Scope.Variables.Add("initialColumn");
            state.TestCaseCollection.Scope.Variables.Add("otherColumn");
            state.TestCaseCollection.Scope.Variables.Add("thirdColumn");
            var firstRow = state.TestCaseCollection.Scope.Content.NewRow();
            firstRow[0] = "a-b-c";
            firstRow[1] = "other";
            firstRow[2] = "1-2";
            state.TestCaseCollection.Scope.Content.Rows.Add(firstRow);
            var secondRow = state.TestCaseCollection.Scope.Content.NewRow();
            secondRow[0] = "a-b";
            secondRow[1] = "other";
            secondRow[2] = "3-4";
            state.TestCaseCollection.Scope.Content.Rows.Add(secondRow);
            var thirdRow = state.TestCaseCollection.Scope.Content.NewRow();
            thirdRow[0] = "a-b-c-d";
            thirdRow[1] = "(none)";
            thirdRow[2] = "5-6-7-8";
            state.TestCaseCollection.Scope.Content.Rows.Add(thirdRow);
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["initialColumn"], Is.TypeOf<string>());
            Assert.That(state.TestCaseCollection.Scope.Content.Rows[0]["thirdColumn"], Is.TypeOf<string>());


            var columns = new string[] { "initialColumn", "thirdColumn" };

            var action = new SplitCaseAction(columns, "-");
            action.Execute(state);
            var dataTable = state.TestCaseCollection.Scope.Content;
            Assert.That(dataTable.Columns, Has.Count.EqualTo(3));
            Assert.That(dataTable.Rows, Has.Count.EqualTo(3));

            Assert.That(dataTable.Rows[0]["otherColumn"], Is.TypeOf<string>());
            Assert.That(dataTable.Rows[0]["initialColumn"], Is.TypeOf<string[]>());
            Assert.That(dataTable.Rows[0]["thirdColumn"], Is.TypeOf<string[]>());

            Assert.That((dataTable.Rows[0]["initialColumn"] as Array).Length, Is.EqualTo(3));
            Assert.That((dataTable.Rows[1]["initialColumn"] as Array).Length, Is.EqualTo(2));
            Assert.That((dataTable.Rows[2]["initialColumn"] as Array).Length, Is.EqualTo(4));

            Assert.That((dataTable.Rows[0]["thirdColumn"] as Array).Length, Is.EqualTo(2));
            Assert.That((dataTable.Rows[1]["thirdColumn"] as Array).Length, Is.EqualTo(2));
            Assert.That((dataTable.Rows[2]["thirdColumn"] as Array).Length, Is.EqualTo(4));
        }
    }
}
