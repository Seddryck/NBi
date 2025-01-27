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

namespace NBi.GenbiL.Testing.Action.Case;

public class MoveCaseActionTest
{
    [Test]
    public void Execute_SecondColumnMoveLeft_ColumnMoved()
    {
        var state = new GenerationState();
        state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("fourthColumn");
        var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
        state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);


        var action = new MoveCaseAction("secondColumn", -1);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(4));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[0], Is.EqualTo("secondColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[1], Is.EqualTo("firstColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[2], Is.EqualTo("thirdColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[3], Is.EqualTo("fourthColumn"));
    }

    [Test]
    public void Execute_SecondColumnMoveRight_ColumnMoved()
    {
        var state = new GenerationState();
        state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("fourthColumn");
        var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
        state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);


        var action = new MoveCaseAction("secondColumn", 1);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(4));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[0], Is.EqualTo("firstColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[1], Is.EqualTo("thirdColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[2], Is.EqualTo("secondColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[3], Is.EqualTo("fourthColumn"));
    }

    [Test]
    public void Execute_ThirdColumnMoveFirst_ColumnMoved()
    {
        var state = new GenerationState();
        state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("fourthColumn");
        var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
        state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);


        var action = new MoveCaseAction("thirdColumn", int.MinValue);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(4));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[0], Is.EqualTo("thirdColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[1], Is.EqualTo("firstColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[2], Is.EqualTo("secondColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[3], Is.EqualTo("fourthColumn"));
    }

    [Test]
    public void Execute_SecondColumnMoveLast_ColumnMoved()
    {
        var state = new GenerationState();
        state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("fourthColumn");
        var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
        state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);


        var action = new MoveCaseAction("secondColumn", int.MaxValue);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(4));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[0], Is.EqualTo("firstColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[1], Is.EqualTo("thirdColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[2], Is.EqualTo("fourthColumn"));
        Assert.That(state.CaseCollection.CurrentScope.Variables.ToArray()[3], Is.EqualTo("secondColumn"));
    }

}
