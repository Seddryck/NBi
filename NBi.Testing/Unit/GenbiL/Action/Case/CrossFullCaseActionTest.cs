using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.Testing.Unit.GenbiL.Action.Case
{
    public class CrossFullCaseActionTest
    {
        [Test]
        public void Execute_ThreeTimesTwo_SixRowsFiveColumns()
        {
            var state = new GenerationState();
            var alphaCase = state.TestCaseCollection.Item("alpha");
            alphaCase.Content.Columns.Add("firstColumn");
            alphaCase.Content.Columns.Add("secondColumn");
            alphaCase.Content.Columns.Add("thirdColumn");
            alphaCase.Variables.Add("firstColumn");
            alphaCase.Variables.Add("secondColumn");
            alphaCase.Variables.Add("thirdColumn");
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

            var betaCase = state.TestCaseCollection.Item("beta");
            betaCase.Content.Columns.Add("fourthColumn");
            betaCase.Content.Columns.Add("fifthColumn");
            betaCase.Variables.Add("fourthColumn");
            betaCase.Variables.Add("fifthColumn");
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

            var action = new CrossFullCaseAction("alpha", "beta");
            action.Execute(state);

            Assert.That(alphaCase.Content.Rows, Has.Count.EqualTo(6));
            Assert.That(alphaCase.Content.Columns, Has.Count.EqualTo(5));
        }
    }
}
