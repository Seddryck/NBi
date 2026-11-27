using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using NBi.Model.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Setting;

public class DefaultActionTest
{
    private const string ConnectionString = "data source='.';uid=myself";

    [Test]
    public void Execute_NewDefaultConnectionString_DefaultAdded()
    {
        var state = new GenerationState();

        var action = new DefaultAction(DefaultType.Everywhere, "ConnectionString", ConnectionString);
        action.Execute(state);
        var target = state.Settings.Defaults.FirstOrDefault(x => x.ApplyTo == SuiteSettings.DefaultScope.Everywhere);
        Assert.That(target, Is.Not.Null);
        Assert.That(target!.ConnectionStringSpecified, Is.True);
        Assert.That(target.ConnectionString.Inline, Is.EqualTo(ConnectionString));
    }

    [Test]
    public void Execute_OverrideExistingDefaultConnectionString_DefaultOverriden()
    {
        var state = new GenerationState();
        state.Settings.Defaults.Add(new Default(SuiteSettings.DefaultScope.Everywhere) { ConnectionString = new ConnectionString() { Inline = "other connString" } });

        var action = new DefaultAction(DefaultType.Everywhere, "ConnectionString", ConnectionString);
        action.Execute(state);
        var target = state.Settings.Defaults.FirstOrDefault(x => x.ApplyTo == SuiteSettings.DefaultScope.Everywhere);
        Assert.That(target, Is.Not.Null);
        Assert.That(target!.ConnectionStringSpecified, Is.True);
        Assert.That(target.ConnectionString.Inline, Is.EqualTo(ConnectionString));
    }
}
