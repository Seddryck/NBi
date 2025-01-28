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

public class SubstituteCaseActionTest
{
    protected GenerationState BuildInitialState()
    {
        var state = new GenerationState();
        state.CaseCollection.CurrentScope.Content.Columns.Add("firstColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("secondColumn");
        state.CaseCollection.CurrentScope.Content.Columns.Add("thirdColumn");
        var firstRow = state.CaseCollection.CurrentScope.Content.NewRow();
        firstRow[0] = "Cell";
        firstRow[1] = "secondCell1";
        firstRow[2] = "Text";
        state.CaseCollection.CurrentScope.Content.Rows.Add(firstRow);
        var secondRow = state.CaseCollection.CurrentScope.Content.NewRow();
        secondRow[0] = "Cell";
        secondRow[1] = "secondCell2";
        secondRow[2] = "";
        state.CaseCollection.CurrentScope.Content.Rows.Add(secondRow);
        var thirdRow = state.CaseCollection.CurrentScope.Content.NewRow();
        thirdRow[0] = "XXX";
        thirdRow[1] = "secondCell3";
        thirdRow[2] = "YYY";
        state.CaseCollection.CurrentScope.Content.Rows.Add(thirdRow);

        return state;
    }

    [Test]
    public void Execute_SecondColumnSubstitutueWithValue_ValueSubstitued()
    {
        var state = BuildInitialState();
        state.CaseCollection.CurrentScope.Content.Rows[2]["secondColumn"] = "(none)";

        var builder = new ValuableBuilder();
        var oldValue = builder.Build(ValuableType.Value, "Cell");
        var newValue = builder.Build(ValuableType.Value, "Text");

        var action = new SubstituteCaseAction("secondColumn", oldValue, newValue);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(3));

        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondText1"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1]["secondColumn"], Is.EqualTo("secondText2"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[2]["secondColumn"], Is.EqualTo("(none)"));
    }

    [Test]
    public void Execute_SecondColumnSubstitutueWithColumn_ValueSubstitued()
    {
        var state = BuildInitialState();

        var builder = new ValuableBuilder();
        var oldValue = builder.Build(ValuableType.Column, "firstColumn");
        var newValue = builder.Build(ValuableType.Column, "thirdColumn");

        var action = new SubstituteCaseAction("secondColumn", oldValue, newValue);
        action.Execute(state);
        Assert.That(state.CaseCollection.CurrentScope.Content.Columns, Has.Count.EqualTo(3));

        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[0]["secondColumn"], Is.EqualTo("secondText1"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[1]["secondColumn"], Is.EqualTo("second2"));
        Assert.That(state.CaseCollection.CurrentScope.Content.Rows[2]["secondColumn"], Is.EqualTo("secondCell3"));
    }

}
