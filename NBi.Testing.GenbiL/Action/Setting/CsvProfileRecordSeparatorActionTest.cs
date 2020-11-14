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

namespace NBi.Testing.GenbiL.Action.Setting
{
    public class CsvProfileRecordSeparatorActionTest
    {
        [Test]
        public void Execute_NewCsvProfileRecordSeparator_ProfileAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileRecordSeparatorAction("\r\n");

            action.Execute(state);
            var target = state.Settings.CsvProfile;
            Assert.That(target, Is.Not.Null);
        }

        
        [Test]
        public void Execute_NewCsvProfile_RecordSeparatorAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileRecordSeparatorAction("\r\n");
            action.Execute(state);
            var target = state.Settings.CsvProfile.RecordSeparator;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo("\r\n"));
        }
        
        [Test]
        public void Execute_OverrideExistingNewCsvProfile_RecordSeparatorOverriden()
        {
            var state = new GenerationState();
            state.Settings.CsvProfile.RecordSeparator = "\r\n";

            var action = new CsvProfileRecordSeparatorAction("\n");
            action.Execute(state);
            var target = state.Settings.CsvProfile.RecordSeparator;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo("\n"));
        }
    }
}
