using NBi.GenbiL;
using NBi.GenbiL.Action;
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

public class TrimCaseActionTest
{
    protected GenerationState BuildInitialState()
    {
        var state = new GenerationState();
        state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
        var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
        firstRow[0] = "Cell ";
        firstRow[1] = " secondCell1 ";
        firstRow[2] = " Text";
        state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
        var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
        secondRow[0] = "Cell";
        secondRow[1] = "(none)";
        secondRow[2] = "";
        state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);

        return state;
    }

    [Test]
    public void Execute_FirstColumnLeftTrimWithValue_ValueTrimmed()
    {
        var state = BuildInitialState();

        var action = new TrimCaseAction(["firstColumn"], DirectionType.Left);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["firstColumn"], Is.EqualTo("Cell "));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1]["firstColumn"], Is.EqualTo("Cell"));
    }

    [Test]
    public void Execute_FirstColumnRightTrimWithValue_ValueTrimmed()
    {
        var state = BuildInitialState();

        var action = new TrimCaseAction(["firstColumn"], DirectionType.Right);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["firstColumn"], Is.EqualTo("Cell"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1]["firstColumn"], Is.EqualTo("Cell"));
    }

    [Test]
    public void Execute_SecondColumnSubstitutueWithValue_ValueTrimmed()
    {
        var state = BuildInitialState();

        var action = new TrimCaseAction(["secondColumn"], DirectionType.Both);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1]["secondColumn"], Is.EqualTo("(none)"));
    }


    [Test]
    public void Execute_AllColumnsTrim_ValueTrimmed()
    {
        var state = BuildInitialState();

        var action = new TrimCaseAction([], DirectionType.Both);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["firstColumn"], Is.EqualTo("Cell"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondCell1"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["thirdColumn"], Is.EqualTo("Text"));

        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1]["firstColumn"], Is.EqualTo("Cell"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1]["secondColumn"], Is.EqualTo("(none)"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1]["thirdColumn"], Is.EqualTo(""));
    }


}
