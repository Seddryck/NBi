using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Consumable;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Consumable;

public class SetConsumableActionTest
{
    [Test]
    public void Execute_NewConsumable_ConsumableAdded()
    {
        var state = new GenerationState();
        state.Consumables.Clear();
        
        var action = new SetConsumableAction("myVar", "2010-10-10");
        action.Execute(state);
        Assert.That(state.Consumables, Has.Count.EqualTo(1));
        Assert.That(state.Consumables.Keys, Has.Member("myVar"));
        Assert.That(state.Consumables["myVar"], Is.EqualTo("2010-10-10"));
    }

    [Test]
    public void Execute_ExistingConsumable_ConsumableUpdated()
    {
        var state = new GenerationState();
        state.Consumables.Clear();
        state.Consumables.Add("myVar", "2012-12-12");

        var action = new SetConsumableAction("myVar", "2010-10-10");
        action.Execute(state);
        Assert.That(state.Consumables, Has.Count.EqualTo(1));
        Assert.That(state.Consumables.Keys, Has.Member("myVar"));
        Assert.That(state.Consumables["myVar"], Is.EqualTo("2010-10-10"));
    }
}
