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
    public class CsvProfileTextQualifierActionTest
    {
        [Test]
        public void Execute_NewCsvProfileTextQualifier_ProfileAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileTextQualifierAction('"');

            action.Execute(state);
            var target = state.Settings.CsvProfile;
            Assert.That(target, Is.Not.Null);
        }

        
        [Test]
        public void Execute_NewCsvProfile_TextQualifierAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileTextQualifierAction('"');
            action.Execute(state);
            var target = state.Settings.CsvProfile.TextQualifier;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo('"'));
        }
        
        [Test]
        public void Execute_OverrideExistingNewCsvProfile_TextQualifierOverriden()
        {
            var state = new GenerationState();
            state.Settings.CsvProfile.TextQualifier = '"';

            var action = new CsvProfileTextQualifierAction('#');
            action.Execute(state);
            var target = state.Settings.CsvProfile.TextQualifier;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo('#'));
        }
    }
}
