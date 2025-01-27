using NBi.GenbiL;
using NBi.GenbiL.Action.Case;
using NBi.GenbiL.Action.Consumable;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Consumable;

public class AutoConsumableActionTest
{
    [Test]
    [TestCase("fr-fr")]
    [TestCase("nl-be")]
    [TestCase("fr-be")]
    [TestCase("de-de")]
    [TestCase("en-us")]
    [TestCase("en-gb")]
    public void Execute_CultureIndependant_ConsumableNow(string culture)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture, false);
        var state = new GenerationState();
        state.Consumables.Clear();

        var action = new AutoConsumableAction(true, new DateTime(2014, 09, 26, 9, 16, 55));
        action.Execute(state);

        Assert.That(state.Consumables["now"], Is.EqualTo("2014-09-26T09:16:55"));
    }

    [Test]
    [TestCase("fr-fr")]
    [TestCase("nl-be")]
    [TestCase("fr-be")]
    [TestCase("de-de")]
    [TestCase("en-us")]
    [TestCase("en-gb")]
    public void Execute_CultureIndependant_ConsumableTime(string culture)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture, false);
        var state = new GenerationState();
        state.Consumables.Clear();

        var action = new AutoConsumableAction(true, new DateTime(2014, 09, 26, 9, 16, 55));
        action.Execute(state);

        Assert.That(DateTime.Parse(state.Consumables["time"].ToString() ?? string.Empty).TimeOfDay, Is.EqualTo(new TimeSpan(9, 16, 55)));
    }

    [Test]
    [TestCase("fr-fr")]
    [TestCase("nl-be")]
    [TestCase("fr-be")]
    [TestCase("de-de")]
    [TestCase("en-us")]
    [TestCase("en-gb")]
    public void Execute_CultureIndependant_ConsumableDate(string culture)
    {
        Thread.CurrentThread.CurrentCulture = new CultureInfo(culture, false);
        var state = new GenerationState();
        state.Consumables.Clear();

        var action = new AutoConsumableAction(true, new DateTime(2014, 09, 26, 9, 16, 55));
        action.Execute(state);

        Assert.That(DateTime.Parse(state.Consumables["today"].ToString() ?? string.Empty).Date, Is.EqualTo(new DateTime(2014, 09, 26)));
    }
    
}
