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
    public class ReferenceActionTest
    {
        private const string ConnectionStringValue = "data source='.';uid=myself";

        [Test]
        public void Execute_NewReferenceConnectionString_ReferenceAdded()
        {
            var state = new GenerationState();

            var action = new ReferenceAction("myRef", "ConnectionString", ConnectionStringValue);
            action.Execute(state);
            var target = state.Settings.References.FirstOrDefault(x => x.Name == "myRef");
            Assert.That(target, Is.Not.Null);
            Assert.That(target!.ConnectionStringSpecified, Is.True);
            Assert.That(target.ConnectionString.Inline, Is.EqualTo(ConnectionStringValue));
        }

        [Test]
        public void Execute_OverrideExistingReferenceConnectionString_ReferenceOverriden()
        {
            var state = new GenerationState();
            state.Settings.References.Add(new ReferenceXml() { Name = "myRef", ConnectionString = new ConnectionStringXml() { Inline = "other connString" } });

            var action = new ReferenceAction("myRef", "ConnectionString", ConnectionStringValue);
            action.Execute(state);
            var target = state.Settings.References.FirstOrDefault(x => x.Name == "myRef");
            Assert.That(target, Is.Not.Null);
            Assert.That(target!.ConnectionStringSpecified, Is.True);
            Assert.That(target.ConnectionString.Inline, Is.EqualTo(ConnectionStringValue));
        }
    }
}
