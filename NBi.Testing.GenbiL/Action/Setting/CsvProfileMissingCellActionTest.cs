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
    public class CsvProfileMissingCellActionTest
    {
        [Test]
        public void Execute_NewCsvProfileMissingCell_ProfileAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileMissingCellAction("NULL");

            action.Execute(state);
            var target = state.Settings.CsvProfile;
            Assert.That(target, Is.Not.Null);
        }

        
        [Test]
        public void Execute_NewCsvProfile_MissingCellAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileMissingCellAction("NULL");
            action.Execute(state);
            var target = state.Settings.CsvProfile.MissingCell;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo("NULL"));
        }
        
        [Test]
        public void Execute_OverrideExistingNewCsvProfile_MissingCellOverriden()
        {
            var state = new GenerationState();
            state.Settings.CsvProfile.MissingCell = "originalValue";

            var action = new CsvProfileMissingCellAction("newValue");
            action.Execute(state);
            var target = state.Settings.CsvProfile.MissingCell;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo("newValue"));
        }
    }
}
