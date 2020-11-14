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
    public class CsvProfileFirstRowHeaderActionTest
    {
        [Test]
        public void Execute_NewCsvProfileFirstRowHeader_ProfileAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileFirstRowHeaderAction(true);

            action.Execute(state);
            var target = state.Settings.CsvProfile;
            Assert.That(target, Is.Not.Null);
        }

        
        [Test]
        public void Execute_NewCsvProfile_FirstRowHeaderAdded()
        {
            var state = new GenerationState();

            var action = new CsvProfileFirstRowHeaderAction(true);
            action.Execute(state);
            var target = state.Settings.CsvProfile.FirstRowHeader;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo(true));
        }
        
        [Test]
        public void Execute_OverrideExistingNewCsvProfile_FirstRowHeaderOverriden()
        {
            var state = new GenerationState();
            state.Settings.CsvProfile.FirstRowHeader = true;

            var action = new CsvProfileFirstRowHeaderAction(false);
            action.Execute(state);
            var target = state.Settings.CsvProfile.FirstRowHeader;
            Assert.That(target, Is.Not.Null);
            Assert.That(target, Is.EqualTo(false));
        }
    }
}
