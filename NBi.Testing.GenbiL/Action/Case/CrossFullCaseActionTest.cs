using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Case;

public class CrossFullCaseActionTest
{
    [Test]
    public void Execute_ThreeTimesTwo_SixRowsFiveColumns()
    {
        var state = new GenerationState();
        var alphaCase = new CaseSet();
        alphaCase.Content.Columns.Add("firstColumn");
        alphaCase.Content.Columns.Add("secondColumn");
        alphaCase.Content.Columns.Add("thirdColumn");
        var firstAlphaRow = alphaCase.Content.NewRow();
        firstAlphaRow[0] = "firstAlphaCell1";
        firstAlphaRow[1] = "secondAlphaCell1";
        firstAlphaRow[2] = "thirdAlphaCell1";
        alphaCase.Content.Rows.Add(firstAlphaRow);
        var secondAlphaRow = alphaCase.Content.NewRow();
        secondAlphaRow[0] = "firstAlphaCell2";
        secondAlphaRow[1] = "secondAlphaCell2";
        secondAlphaRow[2] = "thirdAlphaCell2";
        alphaCase.Content.Rows.Add(secondAlphaRow);
        state.CaseCollection.Add("alpha", alphaCase);

        
        var betaCase = new CaseSet();
        betaCase.Content.Columns.Add("fourthColumn");
        betaCase.Content.Columns.Add("fifthColumn");
        var firstBetaRow = betaCase.Content.NewRow();
        firstBetaRow[0] = "firstBetaCell1";
        firstBetaRow[1] = "secondBetaCell1";
        betaCase.Content.Rows.Add(firstBetaRow);
        var secondBetaRow = betaCase.Content.NewRow();
        secondBetaRow[0] = "firstBetaCell2";
        secondBetaRow[1] = "secondBetaCell2";
        betaCase.Content.Rows.Add(secondBetaRow);
        var thirdBetaRow = betaCase.Content.NewRow();
        thirdBetaRow[0] = "firstBetaCell3";
        thirdBetaRow[1] = "secondBetaCell3";
        betaCase.Content.Rows.Add(thirdBetaRow);
        state.CaseCollection.Add("beta", betaCase);
        state.CaseCollection.CurrentScopeName = "alpha";

        var action = new CrossFullCaseAction("alpha", "beta");
        action.Execute(state);

        Assert.That(alphaCase.Content.Rows, Has.Count.EqualTo(6));
        Assert.That(alphaCase.Content.Columns, Has.Count.EqualTo(5));
    }
}
