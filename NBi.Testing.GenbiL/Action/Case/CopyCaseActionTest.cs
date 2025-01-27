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

public class CopyCaseActionTest
{
    private GenerationState BuildOriginalState()
    {
        var state = new GenerationState();
        var master = new CaseSet(); 
        master.Content.Columns.Add("keyColumn1");
        master.Content.Columns.Add("keyColumn2");
        master.Content.Columns.Add("thirdColumn");
        var firstAlphaRow = master.Content.NewRow();
        firstAlphaRow[0] = "key1";
        firstAlphaRow[1] = "keyA";
        firstAlphaRow[2] = "thirdAlphaCell1";
        master.Content.Rows.Add(firstAlphaRow);
        var secondAlphaRow = master.Content.NewRow();
        secondAlphaRow[0] = "key2";
        secondAlphaRow[1] = "keyB";
        secondAlphaRow[2] = "thirdAlphaCell2";
        master.Content.Rows.Add(secondAlphaRow);
        state.CaseCollection.Add("master", master);
        return state;
    }

    [Test]
    public void Copy_SimpleMaster_CopyIsEffectivelyDone()
    {
        var state = BuildOriginalState();
        var master = state.CaseCollection["master"];

        var action = new CopyCaseAction("master", "copied");
        action.Execute(state);

        Assert.That(state.CaseCollection.ContainsKey("copied"));
        var copied = state.CaseCollection["copied"];

        for (int i = 0; i < master.Content.Rows.Count; i++)
            Assert.That(copied.Content.Rows[i].ItemArray, Is.EqualTo(master.Content.Rows[i].ItemArray));

        Assert.That(copied.Content.Rows, Has.Count.EqualTo(master.Content.Rows.Count));
    }


    [Test]
    public void Copy_SimpleMaster_CopyIsNotReferenceCopy()
    {
        var state = BuildOriginalState();
        var action = new CopyCaseAction("master", "copied");
        action.Execute(state);
        var copied = state.CaseCollection["copied"];
        state.CaseCollection["master"].Content.Clear();

        Assert.That(state.CaseCollection["master"].Content.Rows, Has.Count.EqualTo(0));
        Assert.That(copied.Content.Rows, Has.Count.GreaterThan(0));
    }

    [Test]
    public void Copy_SimpleMasterWithCopiedAlreadyLoaded_CopyIsNotAllowed()
    {
        var state = BuildOriginalState();
        state.CaseCollection.Add("copied", new CaseSet());

        var action = new CopyCaseAction("master", "copied");

        Assert.Throws<ArgumentException>(delegate { action.Execute(state); });
    }

}
