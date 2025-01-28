using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Parser.Valuable;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Case;

public class SeparateCaseActionTest
{
    protected GenerationState BuildInitialState()
    {
        var state = new GenerationState();
        state.CaseCollection.CurrentScope.Content.Columns.Add("initialColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("otherColumn");
        var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
        firstRow[0] = "a-b-c";
        firstRow[1] = "other";
        state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
        var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
        secondRow[0] = "a-b";
        secondRow[1] = "other";
        state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);
        var thirdRow = state.CaseCollection.CurrentScope.Content.NewRow();
        thirdRow[0] = "a-b-c-d";
        thirdRow[1] = "(none)";
        state.CaseCollection.CurrentScope.Content.Rows.Add(thirdRow);

        return state;
    }

    [Test]
    public void Execute_FirstColumn_ValueSeparated()
    {
        var state = BuildInitialState();

        var newColumns = new string[] { "one", "two", "three" };

        var action = new SeparateCaseAction("initialColumn", newColumns, "-");
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(5));

        for (int i = 0; i < 3; i++)
        {
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[i]["one"], Is.EqualTo("a"));
            Assert.That(state.CaseCollection.CurrentScope.Content.Rows[i]["two"], Is.EqualTo("b"));
            switch (i)
            {
                case 0: Assert.That(state.CaseCollection.CurrentScope.Content.Rows[i]["three"], Is.EqualTo("c")); break;
                case 1: Assert.That(state.CaseCollection.CurrentScope.Content.Rows[i]["three"], Is.EqualTo("(none)")); break;
                case 2: Assert.That(state.CaseCollection.CurrentScope.Content.Rows[i]["three"], Is.EqualTo("c-d")); break;
                default:
                    break;
            }
        }
    }
}
