using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NBi.GenbiL.Stateful;

namespace NBi.GenbiL.Testing.Action.Case;

public class ReplaceCaseActionTest
{
    [Test]
    public void Display_LikeOneValue_CorrectString()
    {
        var action = new ReplaceCaseAction("myColumn", "new value", OperatorType.Like, ["first value"], false);
        Assert.That(action.Display, Is.EqualTo("Replacing content of column 'myColumn' with value 'new value' when values like 'first value'"));
    }

    [Test]
    public void Execute_ReplaceSecondColumn_ColumnReplaced()
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
        

        var action = new ReplaceCaseAction("secondColumn", "new cell");
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(3));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(2));
        foreach (DataRow row in state.CaseCollection.CurrentScope.Content.Rows)
            Assert.That(row[1], Is.EqualTo("new cell"));
    }

    [Test]
    public void Execute_ReplaceSecondColumnWithCondition_ColumnReplaced()
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


        var action = new ReplaceCaseAction("secondColumn", "new cell", OperatorType.Like, ["%1"], false);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(3));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(2));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0][1], Is.EqualTo("new cell"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1][1], Is.EqualTo("secondCell2"));
    }

    [Test]
    public void Execute_ReplaceSecondColumnWithConditionAndMultiple_ColumnReplaced()
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
        var thirdRow = state.CaseCollection.CurrentScope.Content.NewRow();
        thirdRow[0] = "firstCell3";
        thirdRow[1] = "(none)";
        thirdRow[2] = "thirdCell3";
        state.CaseCollection.CurrentScope.Content.Rows.Add(thirdRow);


        var action = new ReplaceCaseAction("secondColumn", "new cell", OperatorType.Equal, ["secondCell1", "(none)"], false);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(3));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows, Has.Count.EqualTo(3));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0][1], Is.EqualTo("new cell"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1][1], Is.EqualTo("secondCell2"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[2][1], Is.EqualTo("new cell"));
    }
}
