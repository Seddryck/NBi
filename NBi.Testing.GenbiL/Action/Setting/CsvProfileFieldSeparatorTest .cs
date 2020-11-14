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
    public class CsvProfileFieldSeparatorActionTest
    {
        [Test]
        public void Execute_NewCsvProfileFieldSeparator_ProfileAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileFieldSeparatorAction('|');

            action.Execute(state);
            var target = state.Settings.CsvProfile;
            Assert.That(target, Is.Not.Null);
        }

        
        [Test]
        public void Execute_NewCsvProfile_FieldSeparatorAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileFieldSeparatorAction('|');
            action.Execute(state);
            var target = state.Settings.CsvProfile.FieldSeparator;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo('|'));
        }
        
        [Test]
        public void Execute_OverrideExistingNewCsvProfile_FieldSeparatorOverriden()
        {
            var state = new GenerationState();
            state.Settings.CsvProfile.FieldSeparator = '|';

            var action = new CsvProfileFieldSeparatorAction('\t');
            action.Execute(state);
            var target = state.Settings.CsvProfile.FieldSeparator;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo('\t'));
        }
    }
}
