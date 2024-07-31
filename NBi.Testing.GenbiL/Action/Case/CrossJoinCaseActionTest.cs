using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Case
{
    public class CrossJoinCaseActionTest
    {
        [Test]
        public void Cross_ThreeTimesTwoWithOneCommonColumnName_SixRowsFourColumns()
        {
            var state = new GenerationState();
            var alphaCase = new CaseSet();
            alphaCase.Content.Columns.Add("keyColumn");
            alphaCase.Content.Columns.Add("secondColumn");
            alphaCase.Content.Columns.Add("thirdColumn");
            var firstAlphaRow = alphaCase.Content.NewRow();
            firstAlphaRow[0] = "key1";
            firstAlphaRow[1] = "secondAlphaCell1";
            firstAlphaRow[2] = "thirdAlphaCell1";
            alphaCase.Content.Rows.Add(firstAlphaRow);
            var secondAlphaRow = alphaCase.Content.NewRow();
            secondAlphaRow[0] = "key2";
            secondAlphaRow[1] = "secondAlphaCell2";
            secondAlphaRow[2] = "thirdAlphaCell2";
            alphaCase.Content.Rows.Add(secondAlphaRow);
            state.CaseCollection.Add("alpha", alphaCase);

            var betaCase = new CaseSet();
            betaCase.Content.Columns.Add("keyColumn");
            betaCase.Content.Columns.Add("fifthColumn");
            var firstBetaRow = betaCase.Content.NewRow();
            firstBetaRow[0] = "key1";
            firstBetaRow[1] = "secondBetaCell1";
            betaCase.Content.Rows.Add(firstBetaRow);
            var secondBetaRow = betaCase.Content.NewRow();
            secondBetaRow[0] = "key1";
            secondBetaRow[1] = "secondBetaCell2";
            betaCase.Content.Rows.Add(secondBetaRow);
            var thirdBetaRow = betaCase.Content.NewRow();
            thirdBetaRow[0] = "key2";
            thirdBetaRow[1] = "secondBetaCell3";
            betaCase.Content.Rows.Add(thirdBetaRow);
            state.CaseCollection.Add("beta", betaCase);
            state.CaseCollection.CurrentScopeName = "alpha";

            var action = new CrossJoinCaseAction("alpha", "beta", ["keyColumn"]);
            action.Execute(state);

            Assert.That(alphaCase.Content.Rows, Has.Count.EqualTo(3));
            Assert.That(alphaCase.Content.Columns, Has.Count.EqualTo(4));
        }

        [Test]
        public void Cross_ThreeTimesTwoWithTwoCommonColumnNames_ThreeRowsThreeColumns()
        {
            var state = new GenerationState();
            var alphaCase = new CaseSet();
            alphaCase.Content.Columns.Add("keyColumn1");
            alphaCase.Content.Columns.Add("keyColumn2");
            alphaCase.Content.Columns.Add("thirdColumn");
            var firstAlphaRow = alphaCase.Content.NewRow();
            firstAlphaRow[0] = "key1";
            firstAlphaRow[1] = "keyA";
            firstAlphaRow[2] = "thirdAlphaCell1";
            alphaCase.Content.Rows.Add(firstAlphaRow);
            var secondAlphaRow = alphaCase.Content.NewRow();
            secondAlphaRow[0] = "key2";
            secondAlphaRow[1] = "keyB";
            secondAlphaRow[2] = "thirdAlphaCell2";
            alphaCase.Content.Rows.Add(secondAlphaRow);
            state.CaseCollection.Add("alpha", alphaCase);

            var betaCase = new CaseSet();
            betaCase.Content.Columns.Add("keyColumn1");
            betaCase.Content.Columns.Add("keyColumn2");
            var firstBetaRow = betaCase.Content.NewRow();
            firstBetaRow[0] = "key1";
            firstBetaRow[1] = "keyA";
            betaCase.Content.Rows.Add(firstBetaRow);
            var secondBetaRow = betaCase.Content.NewRow();
            secondBetaRow[0] = "key1";
            secondBetaRow[1] = "keyA";
            betaCase.Content.Rows.Add(secondBetaRow);
            var thirdBetaRow = betaCase.Content.NewRow();
            thirdBetaRow[0] = "key2";
            thirdBetaRow[1] = "keyB";
            betaCase.Content.Rows.Add(thirdBetaRow);
            state.CaseCollection.Add("beta", betaCase);
            state.CaseCollection.CurrentScopeName = "alpha";

            var action = new CrossJoinCaseAction("alpha", "beta", ["keyColumn1", "keyColumn2"]);
            action.Execute(state);

            Assert.That(alphaCase.Content.Rows, Has.Count.EqualTo(3));
            Assert.That(alphaCase.Content.Columns, Has.Count.EqualTo(3));
        }


        [Test]
        public void Cross_MissingMatch_LessRows()
        {
            var state = new GenerationState();
            var alphaCase = new CaseSet();
            alphaCase.Content.Columns.Add("keyColumn1");
            alphaCase.Content.Columns.Add("keyColumn2");
            alphaCase.Content.Columns.Add("thirdColumn");
            var firstAlphaRow = alphaCase.Content.NewRow();
            firstAlphaRow[0] = "key1";
            firstAlphaRow[1] = "keyA";
            firstAlphaRow[2] = "thirdAlphaCell1";
            alphaCase.Content.Rows.Add(firstAlphaRow);
            var secondAlphaRow = alphaCase.Content.NewRow();
            secondAlphaRow[0] = "key2";
            secondAlphaRow[1] = "keyB";
            secondAlphaRow[2] = "thirdAlphaCell2";
            alphaCase.Content.Rows.Add(secondAlphaRow);
            state.CaseCollection.Add("alpha", alphaCase);

            var betaCase = new CaseSet();
            betaCase.Content.Columns.Add("keyColumn1");
            betaCase.Content.Columns.Add("keyColumn2");
            var firstBetaRow = betaCase.Content.NewRow();
            firstBetaRow[0] = "key1";
            firstBetaRow[1] = "keyA";
            betaCase.Content.Rows.Add(firstBetaRow);
            var secondBetaRow = betaCase.Content.NewRow();
            secondBetaRow[0] = "key1";
            secondBetaRow[1] = "keyZ";
            betaCase.Content.Rows.Add(secondBetaRow);
            var thirdBetaRow = betaCase.Content.NewRow();
            thirdBetaRow[0] = "key2";
            thirdBetaRow[1] = "keyB";
            betaCase.Content.Rows.Add(thirdBetaRow);
            state.CaseCollection.Add("beta", betaCase);
            state.CaseCollection.CurrentScopeName = "beta";

            var action = new CrossJoinCaseAction("alpha", "beta", ["keyColumn1", "keyColumn2"]);
            action.Execute(state);

            Assert.That(alphaCase.Content.Rows, Has.Count.EqualTo(2));
            Assert.That(alphaCase.Content.Columns, Has.Count.EqualTo(3));
        }

    }
}
