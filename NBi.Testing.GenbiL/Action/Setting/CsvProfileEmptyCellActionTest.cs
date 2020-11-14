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
    public class CsvProfileEmptyCellActionTest
    {
        [Test]
        public void Execute_NewCsvProfileEmptyCell_ProfileAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileEmptyCellAction("NULL");

            action.Execute(state);
            var target = state.Settings.CsvProfile;
            Assert.That(target, Is.Not.Null);
        }

        
        [Test]
        public void Execute_NewCsvProfile_EmptyCellAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileEmptyCellAction("NULL");
            action.Execute(state);
            var target = state.Settings.CsvProfile.EmptyCell;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo("NULL"));
        }
        
        [Test]
        public void Execute_OverrideExistingNewCsvProfile_EmptyCellOverriden()
        {
            var state = new GenerationState();
            state.Settings.CsvProfile.EmptyCell = "originalValue";

            var action = new CsvProfileEmptyCellAction("newValue");
            action.Execute(state);
            var target = state.Settings.CsvProfile.EmptyCell;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo("newValue"));
        }
    }
}
