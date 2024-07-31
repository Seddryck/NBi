using NBi.GenbiL.Action;
using NBi.GenbiL.Action.Setting;
using NBi.GenbiL.Stateful;
using NUnit.Framework;
using NBi.Xml.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBi.GenbiL.Testing.Action.Setting
{
    public class DefaultActionTest
    {
        private const string ConnectionStringXml = "data source='.';uid=myself";

        [Test]
        public void Execute_NewDefaultConnectionString_DefaultAdded()
        {
            var state = new GenerationState();

            var action = new DefaultAction(DefaultType.Everywhere, "ConnectionString", ConnectionStringXml);
            action.Execute(state);
            var target = state.Settings.Defaults.FirstOrDefault(x => x.ApplyTo == SettingsXml.DefaultScope.Everywhere);
            Assert.That(target, Is.Not.Null);
            Assert.That(target!.ConnectionStringSpecified, Is.True);
            Assert.That(target.ConnectionString.Inline, Is.EqualTo(ConnectionStringXml));
        }

        [Test]
        public void Execute_OverrideExistingDefaultConnectionString_DefaultOverriden()
        {
            var state = new GenerationState();
            state.Settings.Defaults.Add(new DefaultXml(SettingsXml.DefaultScope.Everywhere) { ConnectionString = new ConnectionStringXml() { Inline = "other connString" } });

            var action = new DefaultAction(DefaultType.Everywhere, "ConnectionString", ConnectionStringXml);
            action.Execute(state);
            var target = state.Settings.Defaults.FirstOrDefault(x => x.ApplyTo == SettingsXml.DefaultScope.Everywhere);
            Assert.That(target, Is.Not.Null);
            Assert.That(target!.ConnectionStringSpecified, Is.True);
            Assert.That(target.ConnectionString.Inline, Is.EqualTo(ConnectionStringXml));
        }
    }
}
